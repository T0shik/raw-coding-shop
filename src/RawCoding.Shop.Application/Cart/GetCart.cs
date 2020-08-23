using System.Linq;
using RawCoding.Shop.Domain.Extensions;
using RawCoding.Shop.Domain.Interfaces;

namespace RawCoding.Shop.Application.Cart
{
    [Service]
    public class GetCart
    {
        private readonly ICartManager _cartManager;

        public GetCart(ICartManager cartManager)
        {
            _cartManager = cartManager;
        }

        public object Do(string cartId)
        {
            var cart = _cartManager.GetCartWithStockAndProducts(cartId);

            return new
            {
                Items = cart.Select(x => new
                {
                    x.StockId,
                    x.Qty,
                    ProductName = x.Stock.Product.Name,
                    Image = x.Stock.Product.Images.FirstOrDefault()?.Path,
                    StockDescription = x.Stock.Description,
                    Value = x.Stock.Value.ToMoney(),
                    TotalValue = (x.Qty * x.Stock.Value).ToMoney(),
                }),
                Total = cart.Select(x => x.Qty * x.Stock.Value).Sum().ToMoney()
            };
        }
    }
}