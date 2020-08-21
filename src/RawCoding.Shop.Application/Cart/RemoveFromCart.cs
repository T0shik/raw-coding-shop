using RawCoding.Shop.Domain.Interfaces;

namespace RawCoding.Shop.Application.Cart
{
    [Service]
    public class RemoveFromCart
    {
        private readonly ICartManager _cartManager;

        public RemoveFromCart(ICartManager cartManager)
        {
            _cartManager = cartManager;
        }

        public class Form
        {
            public int StockId { get; set; }
            public int Qty { get; set; }
        }

        public bool Do(Form request)
        {
            if (request.Qty <= 0)
            {
                return false;
            }

            _cartManager.RemoveProduct(request.StockId, request.Qty);

            return true;
        }
    }
}
