using Microsoft.EntityFrameworkCore;
using RawCoding.Shop.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RawCoding.Shop.Domain.Interfaces;

namespace RawCoding.Shop.Database
{
    public class StockManager : IStockManager
    {
        private readonly ApplicationDbContext _ctx;

        public StockManager(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public Task<int> CreateStock(Stock stock)
        {
            _ctx.Stock.Add(stock);

            return _ctx.SaveChangesAsync();
        }

        public Task<int> DeleteStock(int id)
        {
            var stock = _ctx.Stock.FirstOrDefault(x => x.Id == id);

            _ctx.Stock.Remove(stock);

            return _ctx.SaveChangesAsync();
        }

        public Task<int> UpdateStockRange(IEnumerable<Stock> stockList)
        {
            _ctx.Stock.UpdateRange(stockList);

            return _ctx.SaveChangesAsync();
        }

        public bool HasStock(int stockId, int qty)
        {
            var stock = _ctx.Stock?.AsNoTracking()
                .FirstOrDefault(x => x.Id == stockId);

            if (stock == null) return false;

            return stock.Qty >= qty;
        }

        public Stock GetStockWithProduct(int stockId)
        {
            return _ctx.Stock
                .AsNoTracking()
                .Include(x => x.Product)
                .FirstOrDefault(x => x.Id == stockId);
        }

        public IEnumerable<Stock> GetStockWithProduct(int[] stockIds)
        {
            return _ctx.Stock
                .AsNoTracking()
                .Where(x => stockIds.Contains(x.Id))
                .Include(x => x.Product)
                .ToList();
        }
    }
}