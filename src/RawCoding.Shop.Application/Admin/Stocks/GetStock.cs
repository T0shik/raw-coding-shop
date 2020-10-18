using System.Collections.Generic;
using RawCoding.Shop.Domain.Interfaces;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Application.Admin.Stocks
{
    [Service]
    public class GetStock
    {
        private readonly IStockManager _stockManager;

        public GetStock(IStockManager stockManager)
        {
            _stockManager = stockManager;
        }

        public IEnumerable<Stock> ForProduct(int id)
        {
            return _stockManager.ListProductStock(id);
        }
    }
}
