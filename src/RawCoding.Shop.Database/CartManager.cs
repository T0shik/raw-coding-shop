using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RawCoding.Shop.Domain.Interfaces;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Database
{
    public class CartManager : ICartManager
    {
        private readonly ApplicationDbContext _ctx;

        public CartManager(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public Task<int> UpdateCart(IList<CartProduct> cartProducts)
        {
            _ctx.UpdateRange(cartProducts);
            return _ctx.SaveChangesAsync();
        }

        public async Task<int> RemoveStock(int stockId, string cartId)
        {
            var stock = _ctx.CartProducts
                .FirstOrDefault(x => x.StockId == stockId
                                     && x.CartId == cartId);

            if (stock == null)
            {
                return -1;
            }

            _ctx.CartProducts.Remove(stock);
            await _ctx.SaveChangesAsync();

            return stock.Qty;
        }

        public IList<CartProduct> GetCart(string cartId)
        {
            return _ctx.CartProducts
                .Where(x => x.CartId == cartId && !x.Complete)
                .ToList();
        }

        public IEnumerable<CartProduct> GetCartWithStockAndProducts(string cartId)
        {
            return _ctx.CartProducts
                .Where(x => x.CartId == cartId && !x.Complete)
                .Include(x => x.Stock)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Images)
                .AsNoTracking()
                .ToList();
        }


        public void ClearCart()
        {
            throw new NotImplementedException();
        }

        public void AddCustomerInformation(CustomerInformation customer)
        {
            throw new NotImplementedException();
        }

        public CustomerInformation GetCustomerInformation()
        {
            throw new NotImplementedException();
        }
    }
}