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

        private async Task<Cart> CreateCart(string cartId)
        {
            var cart = new Cart
            {
                UserId = cartId,
            };
            _ctx.Add(cart);
            await _ctx.SaveChangesAsync();
            return cart;
        }

        public Task<int> UpdateCart(Cart cart)
        {
            _ctx.Carts.Update(cart);
            return _ctx.SaveChangesAsync();
        }

        public async Task<int> RemoveStock(int stockId, string userId)
        {
            var cart = _ctx.Carts.AsNoTracking().FirstOrDefault(x => x.UserId == userId);

            if (cart == null)
            {
                return -1;
            }

            var stock = _ctx.CartProducts
                .FirstOrDefault(x => x.StockId == stockId && x.CartId == cart.Id);

            if (stock == null)
            {
                return -1;
            }

            _ctx.CartProducts.Remove(stock);
            await _ctx.SaveChangesAsync();

            return stock.Qty;
        }

        public async Task<int> GetCartId(string userId)
        {
            var cart = _ctx.Carts?
                           .AsNoTracking()
                           .FirstOrDefault(x => x.UserId == userId && !x.Closed)
                       ?? await CreateCart(userId);

            return cart.Id;
        }

        public Task<Cart> GetCartById(int cartId)
        {
            return _ctx.Carts
                .Include(x => x.Products)
                .ThenInclude(x => x.Stock)
                .FirstOrDefaultAsync(x => x.Id == cartId);
        }

        public Task<Cart> GetCartByUserId(string userId)
        {
            var cart = _ctx.Carts
                .Where(x => x.UserId == userId && !x.Closed)
                .Include(x => x.Products)
                .FirstOrDefault();

            return cart == null ? CreateCart(userId) : Task.FromResult(cart);
        }

        public Task<Cart> GetCartWithStock(string userId)
        {
            return _ctx.Carts
                .AsNoTracking()
                .Where(x => x.UserId == userId && !x.Closed)
                .Include(x => x.Products)
                .ThenInclude(x => x.Stock)
                .FirstOrDefaultAsync();
        }

        public IList<CartProduct> GetCartProducts(int cartId)
        {
            return _ctx.CartProducts
                .Where(x => x.CartId == cartId)
                .Include(x => x.Stock)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Images)
                .AsNoTracking()
                .ToList();
        }
    }
}