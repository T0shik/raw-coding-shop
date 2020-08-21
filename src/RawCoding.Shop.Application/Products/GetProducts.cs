using System.Collections.Generic;
using System.Linq;
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
                    Value = $"£ {x.Value * 0.01:N2}",

                    Stock = x.Stock.Sum(y => y.Qty),
                    Images = x.Images.Select(y => y.Path)
                        .Take(2)
                        .ToList()
                });
        }

        public class ProductViewModel
        {
            public string Name { get; set; }
            public string Series { get; set; }
            public string Value { get; set; }
            public string Slug { get; set; }
            public int Stock { get; set; }
            public List<string> Images { get; set; }
        }
    }
}