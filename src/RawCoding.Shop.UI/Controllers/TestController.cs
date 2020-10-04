using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotLiquid;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using RawCoding.Shop.Application.Orders;
using RawCoding.Shop.Domain.Extensions;
using RawCoding.Shop.Domain.Models;
using RawCoding.Shop.UI.Workers.Email;

namespace RawCoding.Shop.UI.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Index(
            [FromServices] IEmailSink emailSink,
            [FromServices] IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                await emailSink.SendAsync(new EmailRequest {Subject = "Test", To = "info@raw-coding.dev"});
            }

            return Ok();
        }

        [HttpPost("order")]
        public async Task<IActionResult> Order(
            [FromServices] IEmailSink emailSink,
            [FromServices] IWebHostEnvironment env,
            [FromServices] GetOrder getOrder)
        {
            if (env.IsProduction())
                return Ok();

            var order = getOrder.Do("dummy");
            var templatePath = Path.Combine(env.WebRootPath, "email-templates", "order.liquid");
            var templateString = await System.IO.File.ReadAllTextAsync(templatePath);
            var template = Template.Parse(templateString);

            await emailSink.SendAsync(new EmailRequest
            {
                Subject = "Test",
                To = "info@raw-coding.dev",
                Message = template.Render(Hash.FromAnonymousObject(ToAnon(order))),
                Html = true,
            });

            return Ok();
        }

        private static object ToAnon(Order order) => new
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
                DefaultImage = x.Stock.Product.Images[0],
            }),
        };
    }
}