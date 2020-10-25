using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotLiquid;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using RawCoding.Shop.Application.Emails;
using RawCoding.Shop.Application.Orders;
using RawCoding.Shop.Application.Projections;
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
                await emailSink.SendAsync(new SendEmailRequest {Subject = "Test", To = "info@raw-coding.dev"});
            }

            return Ok();
        }

        [HttpPost("order")]
        public async Task<IActionResult> Order(
            [FromServices] IEmailSink emailSink,
            [FromServices] IWebHostEnvironment env,
            [FromServices] GetOrder getOrder,
            [FromServices] IEmailTemplateFactory etf)
        {
            if (env.IsProduction())
                return Ok();

            var order = getOrder.ForUserById("dummy");
            // var templatePath = Path.Combine(env.WebRootPath, "email-templates", "order.liquid");
            // var templateString = await System.IO.File.ReadAllTextAsync(templatePath);
            // var template = Template.Parse(templateString);

            await emailSink.SendAsync(new SendEmailRequest
            {
                Subject = $"Order Placed, {order.Id}",
                To = "info@raw-coding.dev",
                Message = await etf.RenderShippingConfirmationAsync(order),// template.Render(Hash.FromAnonymousObject(OrderProjections.Project(order))),
                Html = true,
            });

            return Ok();
        }
    }
}