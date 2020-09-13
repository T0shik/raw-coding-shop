using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<object> Do(string userId)
        {
            var cartId = await _cartManager.GetCartId(userId);
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

        public async Task<IEnumerable<T>> Do<T>(string userId, Func<CartProduct, T> selector)
        {
            var cart = await _cartManager.GetCart(userId);
            if (cart == null)
            {
                return Enumerable.Empty<T>();
            }

            return _cartManager.GetCartProducts(cart.Id).Select(selector);
        }


        public Task<Domain.Models.Cart> Get(string cartId)
        {
            return _cartManager.GetCart(cartId);
        }

        public Domain.Models.Cart Full(string cartId)
        {
            return _cartManager.GetCartFull(cartId);
        }

        public Task<int> Id(string userId)
        {
            return _cartManager.GetCartId(userId);
        }

        public Task<Domain.Models.Cart> WithStock(string userId)
        {
            return _cartManager.GetCartWithStock(userId);
        }
    }
}