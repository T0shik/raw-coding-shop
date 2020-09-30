using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using RawCoding.Shop.UI.Pages;
using RawCoding.Shop.UI.Workers.Email;
using RazorEngine;
using RazorEngine.Templating;
using RazorEngineCore;

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
            [FromServices] IWebHostEnvironment env)
        {
            var file = Path.Combine(env.ContentRootPath, "Pages", "Shared", "Emails", "_OrderConfirmation.cshtml");
            using (var fileStreams = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                var contents = fileReader.ReadAsync();
            }

            var result = Engine.Razor.RunCompile(template, "templateKey", null, new { Name = "World" });

            if (env.IsDevelopment())
            {
                await emailSink.SendAsync(new EmailRequest
                {
                    Subject = "Test",
                    To = "info@raw-coding.dev",
                    Message =
                });
            }

            return Ok();
        }
    }
}