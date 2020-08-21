using System.Collections.Generic;
using RawCoding.Shop.Domain.Interfaces;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Application.Admin.Stocks
{
    [Service]
    public class GetStock
    {
        private readonly IProductManager _productManager;

        public GetStock(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public IEnumerable<Product> Do()
        {
            return _productManager.GetProducts();
        }

        public class StockViewModel
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public int Qty { get; set; }
        }
    }
}
