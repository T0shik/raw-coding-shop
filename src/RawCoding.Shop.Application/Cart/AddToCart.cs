using RawCoding.Shop.Domain.Models;
using RawCoding.Shop.Domain.Interfaces;

namespace RawCoding.Shop.Application.Cart
{
    [Service]
    public class AddToCart
    {
        private readonly ICartManager _cartManager;
        private readonly IStockManager _stockManager;

        public AddToCart(
            ICartManager cartManager,
            IStockManager stockManager)
        {
            _cartManager = cartManager;
            _stockManager = stockManager;
        }

        public class Form
        {
            public int StockId { get; set; }
            public int Qty { get; set; }
        }

        public bool Do(Form request)
        {
            // service responsibility
            if (!_stockManager.EnoughStock(request.StockId, request.Qty))
            {
                return false;
            }

            var stock = _stockManager.GetStockWithProduct(request.StockId);

            var cartProduct = new CartProduct
            {
                ProductId = stock.ProductId,
                ProductName = stock.Product.Name,
                StockId = stock.Id,
                Qty = request.Qty,
                Value = stock.Product.Value,
            };

            //todo this will be different
            _cartManager.AddProduct(cartProduct);

            return true;
        }
    }
}
