using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RawCoding.Shop.Application.Cart;
using RawCoding.Shop.Domain.Interfaces;
using Stripe;
using Stripe.Checkout;

namespace RawCoding.Shop.UI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize(Policy = ShopConstants.Policies.Customer)]
    public class CartController : ControllerBase
    {
        private readonly ICartManager _cartManager;

        public CartController(ICartManager cartManager)
        {
            _cartManager = cartManager;
        }

        [HttpGet("guest-auth")]
        [AllowAnonymous]
        public async Task<IActionResult> Auth(string returnUrl = null)
        {
            var userId = Guid.NewGuid().ToString();
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, ShopConstants.Roles.Guest),
                new Claim(ClaimTypes.NameIdentifier, userId),
            }, ShopConstants.Schemas.Guest);

            var claimsPrinciple = new ClaimsPrincipal(identity);

            var signInAsync = HttpContext.SignInAsync(
                ShopConstants.Schemas.Guest,
                claimsPrinciple,
                new AuthenticationProperties
                {
                    IsPersistent = true
                });

            await signInAsync;

            return Redirect(returnUrl ?? "/");
        }

        [HttpGet("checkout")]
        public async Task<IActionResult> Do(
            [FromServices] IOptionsMonitor<StripeSettings> optionsMonitor,
            [FromServices] GetCart getCart)
        {
            StripeConfiguration.ApiKey = optionsMonitor.CurrentValue.SecretKey;
            var userId = User?.Claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                ShippingAddressCollection = new SessionShippingAddressCollectionOptions
                {
                    AllowedCountries = new List<string>
                    {
                        "CA",
                        "GB",
                    },
                },
                LineItems = (await getCart.Do(userId, x => new SessionLineItemOptions
                {
                    Amount = x.Stock.Value,
                    Currency = "gbp",
                    Name = x.Stock.Product.Name,
                    Description = x.Stock.Description,
                    Quantity = x.Qty,
                })).ToList(),
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

            return Ok(session.Id);
        }

        [HttpGet]
        public async Task<IActionResult> GetCart([FromServices] GetCart getCart)
        {
            var userId = User?.Claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Cookie Policy not accepted");
            }

            return Ok(await getCart.Do(userId));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCart(
            [FromBody] UpdateCart.Form request,
            [FromServices] UpdateCart updateCart)

        {
            var userId = User?.Claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Cookie Policy not accepted");
            }

            request.UserId = userId;
            var result = await updateCart.Do(request);

            if (result.Success)
                return Ok(result.Message);

            return BadRequest(result.Message);
        }

        [HttpDelete("{stockId}")]
        public async Task<IActionResult> UpdateCart(int stockId, [FromServices] RemoveFromCart removeFromCart)
        {
            var userId = User?.Claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Cookie Policy not accepted");
            }

            var result = await removeFromCart.Do(new RemoveFromCart.Form
            {
                CartId = userId,
                StockId = stockId
            });

            if (result.Success)
                return Ok(result.Message);

            return BadRequest(result.Message);
        }
    }
}