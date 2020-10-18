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
        private readonly IProductManager _productManager;

        public UpdateStock(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public class StockForm
        {
            public int Qty { get; set; }
            public string Description { get; set; }
            public int Value { get; set; }
        }

        public Task ForProduct(int productId, IEnumerable<StockForm> stocks)
        {
            return _productManager.UpdateProductStock(productId, stocks.Select(x => new Stock
            {
                Qty = x.Qty,
                Description = x.Description,
                Value = x.Value,
            }));
        }
    }
}