using System.Collections.Generic;
using System.Linq;
using RawCoding.Shop.Domain.Interfaces;

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

        public IEnumerable<ProductViewModel> Do()
        {
            return _productManager.GetProducts(x =>
                new ProductViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    Stock = x.Stock.Select(y => new StockViewModel
                    {
                        Id = y.Id,
                        Description = y.Description,
                        Qty = y.Qty
                    })
                });
        }

        public class StockViewModel
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public int Qty { get; set; }
        }

        public class ProductViewModel
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public IEnumerable<StockViewModel> Stock { get; set; }
        }
    }
}
