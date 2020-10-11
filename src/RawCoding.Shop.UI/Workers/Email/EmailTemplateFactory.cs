using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotLiquid;
using Microsoft.AspNetCore.Hosting;
using RawCoding.Shop.Application.Emails;
using RawCoding.Shop.Domain.Extensions;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.UI.Workers.Email
{
    public class EmailTemplateFactory : IEmailTemplateFactory
    {
        private readonly IWebHostEnvironment _env;
        private Dictionary<string, Template> TemplateCache { get; } = new Dictionary<string, Template>();

        public EmailTemplateFactory(IWebHostEnvironment env)
        {
            _env = env;
        }

        public Task<string> RenderOrderAsync(Order order) =>
            RenderTemplate("order", new
            {
                order.Id,
                order.Status,

                order.Cart.Name,
                order.Cart.Email,
                order.Cart.Phone,

                order.Cart.Address1,
                order.Cart.Address2,
                order.Cart.City,
                order.Cart.Country,
                order.Cart.PostCode,
                order.Cart.State,

                Products = order.Cart.Products.Select(x => new
                {
                    x.Stock.Product.StockDescription,
                    StockText = x.Stock.Description,

                    x.Qty,
                    x.Stock.Value,
                    Total = (x.Qty * x.Stock.Value).ToMoney(),

                    x.Stock.Product.Name,
                    x.Stock.Product.Series,
                    x.Stock.Product.Description,
                    DefaultImage = x.Stock.Product.Images[0].Path,
                }),
            });

        private async Task<string> RenderTemplate(string templateName, object seed)
        {
            var templatePath = Path.Combine(_env.WebRootPath, "email-templates", $"{templateName}.liquid");
            if (!TemplateCache.TryGetValue(templatePath, out var template))
            {
                var templateString = await File.ReadAllTextAsync(templatePath);
                TemplateCache[templatePath] = template = Template.Parse(templateString);
            }

            return template.Render(Hash.FromAnonymousObject(seed));
        }
    }
}