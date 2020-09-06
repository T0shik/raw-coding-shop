using System;
using System.Collections.Generic;
using System.Linq;
using RawCoding.Shop.Domain.Extensions;
using RawCoding.Shop.Domain.Interfaces;
using RawCoding.Shop.Domain.Models;

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
            var cartProducts = _cartManager.GetCartProducts(cartId);

            return new
            {
                Items = cartProducts.Select(x => new
                {
                    x.StockId,
                    x.Qty,
                    ProductName = x.Stock.Product.Name,
                    Image = x.Stock.Product.Images.FirstOrDefault()?.Path,
                    StockDescription = x.Stock.Description,
                    Value = x.Stock.Value.ToMoney(),
                    TotalValue = (x.Qty * x.Stock.Value).ToMoney(),
                }),
                Total = cartProducts.Select(x => x.Qty * x.Stock.Value).Sum().ToMoney()
            };
        }

        public IEnumerable<T> Do<T>(string cartId, Func<CartProduct, T> selector)
        {
            return _cartManager.GetCartProducts(cartId).Select(selector);
        }
    }
}