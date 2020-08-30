using System.Collections.Generic;
using System.Linq;
using RawCoding.Shop.Domain.Extensions;
using RawCoding.Shop.Domain.Interfaces;

namespace RawCoding.Shop.Application.Products
{
    [Service]
    public class GetProducts
    {
        private readonly IProductManager _productManager;

        public GetProducts(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public IEnumerable<ProductViewModel> Do()
        {
            return _productManager.GetProducts()
                .Select(x => new ProductViewModel
                {
                    Name = x.Name,
                    Slug = x.Slug,
                    Series = x.Series,
                    MultiplePrices = x.Stock.Select(y => y.Value).Distinct().Count() > 1,
                    Value = x.Stock.Min(y => y.Value).ToMoney(),

                    LimitedStock = x.Stock.Any(y => y.Qty == 0),
                    OutOfStock = x.Stock.All(y => y.Qty == 0),
                    Images = x.Images.Select(y => y.Path)
                        .Take(2)
                        .ToList()
                });
        }

        public class ProductViewModel
        {
            public string Name { get; set; }
            public string Series { get; set; }
            public bool MultiplePrices { get; set; }
            public string Value { get; set; }
            public string Slug { get; set; }
            public bool LimitedStock { get; set; }
            public bool OutOfStock { get; set; }
            public List<string> Images { get; set; }
        }
    }
}