using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RawCoding.Shop.Domain.Interfaces;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Application.Admin.Stocks
{
    [Service]
    public class UpdateStock
    {
        private readonly IStockManager _stockManager;

        public UpdateStock(IStockManager stockManager)
        {
            _stockManager = stockManager;
        }

        public async Task<IEnumerable<object>> Do(IEnumerable<Stock> stocks)
        {
            await _stockManager.UpdateStockRange(stocks);

            return stocks.Select(x => new
            {
                x.Id,
                x.Qty,
                x.ProductId
            });
        }
    }
}
