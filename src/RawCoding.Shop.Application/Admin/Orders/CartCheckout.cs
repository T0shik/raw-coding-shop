using System.Threading.Tasks;
using RawCoding.Shop.Domain.Interfaces;

namespace RawCoding.Shop.Application.Admin.Orders
{
    [Service]
    public class CartCheckout
    {
        private readonly ICartManager _cartManager;

        public CartCheckout(ICartManager cartManager)
        {
            _cartManager = cartManager;
        }

        public async Task Complete(int cartId)
        {
            var cart = await _cartManager.GetCartById(cartId);

            foreach (var product in cart.Products)
            {
                product.Stock.Qty -= product.Qty;
            }

            cart.Closed = true;

            await _cartManager.UpdateCart(cart);
        }

        public async Task Cancel(int cartId)
        {
            var cart = await _cartManager.GetCartById(cartId);

            cart.Closed = true;

            await _cartManager.UpdateCart(cart);
        }
    }
}