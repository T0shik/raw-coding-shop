using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RawCoding.Shop.Application.Cart;
using RawCoding.Shop.Application.Orders;
using RawCoding.Shop.Domain.Models;
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

        [HttpPost]
        public async Task<IActionResult> Index(
            [FromServices] IOptionsMonitor<StripeSettings> optionsMonitor,
            [FromServices] CreateOrder createOrder,
            [FromServices] GetCart getCart)
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
                else if (stripeEvent.Type == Events.ChargeSucceeded)
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
                    var checkoutSession = stripeEvent.Data.Object as Session;

                    var order = new Domain.Models.Order
                    {
                        StripeReference = checkoutSession.Id,
                        CartId = await getCart.Id(checkoutSession.Metadata["user_id"]),
                    };
                    await createOrder.Do(order);
                    _logger.LogInformation("created order {0},{1}: {2}", order.Id, Environment.NewLine,
                        JsonConvert.SerializeObject(order));
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