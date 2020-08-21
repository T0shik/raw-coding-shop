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

        public bool EnoughStock(int stockId, int qty)
        {
            return _ctx.Stock.FirstOrDefault(x => x.Id == stockId).Qty >= qty;
        }

        public Stock GetStockWithProduct(int stockId)
        {
            return _ctx.Stock
                .Include(x => x.Product)
                .FirstOrDefault(x => x.Id == stockId);
        }
    }
}