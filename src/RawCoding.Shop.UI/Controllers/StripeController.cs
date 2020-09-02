using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace RawCoding.Shop.UI.Controllers
{
    [ApiController]
    [Route("api/stripe")]
    public class StripeController : ControllerBase
    {
        private readonly ILogger<StripeController> _logger;

        public StripeController(ILogger<StripeController> logger)
        {
            _logger = logger;
        }

        // Stripe Api Events: https://stripe.com/docs/api/events/types

        [HttpPost("")]
        public async Task<IActionResult> Index(
            [FromServices] IOptionsMonitor<StripeSettings> optionsMonitor)
        {
            StripeConfiguration.ApiKey = optionsMonitor.CurrentValue.SecretKey;
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"],
                    optionsMonitor.CurrentValue.SigningSecret);

                _logger.LogInformation("received stripe event {0} {1}", Environment.NewLine, json);

                if (stripeEvent.Type == Events.PaymentIntentCreated)
                {
                    var paymentMethod = stripeEvent.Data.Object as PaymentIntent;
                }
                if (stripeEvent.Type == Events.ChargeSucceeded)
                {
                    var paymentMethod = stripeEvent.Data.Object as Charge;
                }
                else if (stripeEvent.Type == Events.PaymentMethodAttached)
                {
                    var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                }
                else if (stripeEvent.Type == Events.CustomerCreated)
                {
                    var paymentMethod = stripeEvent.Data.Object as Customer;
                }
                else if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var paymentMethod = stripeEvent.Data.Object as Session;
                }
                else
                {
                    return BadRequest();
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}