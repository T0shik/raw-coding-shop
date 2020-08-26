using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RawCoding.Shop.Application.Cart;
using RawCoding.Shop.Domain.Interfaces;

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
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, ShopConstants.Roles.Guest),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            }, ShopConstants.Schemas.Guest);

            var claimsPrinciple = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                ShopConstants.Schemas.Guest,
                claimsPrinciple,
                new AuthenticationProperties
                {
                    IsPersistent = true
                });

            return Redirect(returnUrl ?? "/");
        }

        [HttpGet]
        public object GetCart([FromServices] GetCart getCart)
        {
            var userId = User?.Claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Enumerable.Empty<object>();
            }

            return getCart.Do(userId);
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

            request.CartId = userId;
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