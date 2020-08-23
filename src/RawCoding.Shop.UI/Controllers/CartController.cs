using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
    }
}