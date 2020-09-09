using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RawCoding.Shop.Application.Cart;
using Stripe;
using Stripe.Checkout;

namespace RawCoding.Shop.UI.Pages.Payment
{
    public class Redirect : PageModel
    {
        public string SessionId { get; set; }

        public void OnGet(
            [FromServices] IOptionsMonitor<StripeSettings> optionsMonitor,
            [FromServices] GetCart getCart)
        {
            StripeConfiguration.ApiKey = optionsMonitor.CurrentValue.SecretKey;
            var userId = User?.Claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            var cart = getCart.Full(userId);
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    ReceiptEmail = cart.Email,
                    Shipping = new ChargeShippingOptions
                    {
                        Name = cart.Name,
                        Phone = cart.Phone,
                        Address = new AddressOptions
                        {
                            Line1 = cart.Address1,
                            Line2 = cart.Address2,
                            City = cart.City,
                            Country = cart.Country,
                            PostalCode = cart.PostCode,
                            State = cart.State,
                        }
                    }
                },
                LineItems = cart.Products.Select(x => new SessionLineItemOptions
                {
                    Amount = x.Stock.Value,
                    Currency = "gbp",
                    Name = x.Stock.Product.Name,
                    Description = x.Stock.Description,
                    Quantity = x.Qty,
                }).ToList(),
                Mode = "payment",
                SuccessUrl = "https://localhost:5001/payment/success",
                CancelUrl = "https://localhost:5001/payment/canceled",

                Metadata = new Dictionary<string, string>
                {
                    {"user_id", userId},
                },
            };

            var service = new SessionService();
            var session = service.Create(options);
            SessionId = session.Id;
        }
    }
}