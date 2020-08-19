using System.Collections.Generic;
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

        public IEnumerable<object> Do() =>
            _cartManager.GetCart(x => new
            {
                Name = x.ProductName,
                x.Value,
                RealValue = x.Value,
                x.StockId,
                x.Qty
            });
    }
}