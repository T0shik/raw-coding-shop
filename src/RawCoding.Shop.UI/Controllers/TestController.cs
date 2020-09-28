using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
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
                await emailSink.SendAsync(new EmailRequest {Subject = "Test", To = "atwieslander@gmail.com"});
            }

            return Ok();
        }
    }
}