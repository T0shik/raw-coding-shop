using System.Linq;
using System.Threading.Tasks;
using RawCoding.Shop.Domain.Models;
using RawCoding.Shop.Domain.Interfaces;

namespace RawCoding.Shop.Application.Cart
{
    [Service]
    public class UpdateCart
    {
        private readonly ICartManager _cartManager;
        private readonly IStockManager _stockManager;

        public UpdateCart(
            ICartManager cartManager,
            IStockManager stockManager)
        {
            _cartManager = cartManager;
            _stockManager = stockManager;
        }

        public class Form
        {
            public string CartId { get; set; }
            public int StockId { get; set; }
            public int Qty { get; set; }
        }

        public async Task<BaseResponse> Do(Form request)
        {
            var stock = _stockManager.GetStock(request.StockId);
            if (stock == null)
            {
                return new BaseResponse("Product not found", false);
            }

            if (stock.Qty < request.Qty)
            {
                return new BaseResponse("Not Enough Stock", false);
            }

            var cart = _cartManager.GetCart(request.CartId);

            var product = cart.FirstOrDefault(x => x.StockId == request.StockId);
            if (product == null)
            {
                cart.Add(new CartProduct
                {
                    CartId = request.CartId,
                    StockId = request.StockId,
                    Qty = request.Qty,
                });
            }
            else
            {
                product.Qty += request.Qty;
            }

            stock.Qty -= request.Qty;

            await _cartManager.UpdateCart(cart);
            await _stockManager.UpdateStockRange(new[] {stock});

            return new BaseResponse("Product Added");
        }
    }
}