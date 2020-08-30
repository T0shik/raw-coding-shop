using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Stripe;

namespace RawCoding.Shop.UI.Pages
{
    public class Checkout : PageModel
    {
        public string ClientSecret { get; set; }
        public string PublicKey { get; set; }

        public void OnGet([FromServices] IOptionsMonitor<StripeSettings> optionsMonitor)
        {
            StripeConfiguration.ApiKey = optionsMonitor.CurrentValue.SecretKey;
            var options = new PaymentIntentCreateOptions
            {
                Amount = 1099,
                Currency = "gbp",

                Metadata = new Dictionary<string, string>
                {
                    {"integration_check", "accept_a_payment"},
                },
            };

            var service = new PaymentIntentService();
            var paymentIntent = service.Create(options);

            ClientSecret = paymentIntent.ClientSecret;
            PublicKey = optionsMonitor.CurrentValue.PublicKey;
        }
    }
}