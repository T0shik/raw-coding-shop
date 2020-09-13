using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RawCoding.Shop.Application.Cart;
using RawCoding.Shop.Application.Orders;
using RawCoding.Shop.UI.Extensions;
using Stripe;

namespace RawCoding.Shop.UI.Pages.Checkout
{
    public class Payment : PageModel
    {
        public string ClientSecret { get; set; }

        public async Task<IActionResult> OnGet(
            [FromServices] IOptionsMonitor<StripeSettings> optionsMonitor,
            [FromServices] GetCart getCart)
        {
            var userId = User.GetUserId();
            StripeConfiguration.ApiKey = optionsMonitor.CurrentValue.SecretKey;

            var cartItems = (await getCart.Do(userId, c => c)).ToList();
            if (cartItems.Count <= 0)
            {
                return RedirectToPage("/Index");
            }

            var paymentIntents = new PaymentIntentService();
            var paymentIntent = await paymentIntents.CreateAsync(new PaymentIntentCreateOptions
            {
                Amount = cartItems.Sum(x => x.Qty * x.Stock.Value),
                Currency = "gbp",
            });

            ClientSecret = paymentIntent.ClientSecret;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(
            string paymentId,
            [FromServices] CreateOrder createOrder,
            [FromServices] GetCart getCart)
        {
            var order = new Domain.Models.Order
            {
                StripeReference = paymentId,
                CartId = await getCart.Id(User.GetUserId()),
            };
            await createOrder.Do(order);

            return RedirectToPage("/Checkout/Success", new {orderId = order.Id});
        }
    }
}