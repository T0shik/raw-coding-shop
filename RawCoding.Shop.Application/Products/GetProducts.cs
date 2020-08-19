using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
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
            var a = _productManager.GetProducts(x => new ProductViewModel
            {
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                Value = $"£ {(x.Value * 0.01):N2}",

                Stock = x.Stock.AsQueryable().Sum(y => y.Qty),
                Images = x.Images.AsQueryable().Select(y => y.Path)
            });
            return a;
        }

        public class ProductViewModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }
            public string Slug { get; set; }
            public int Stock { get; set; }
            public IEnumerable<string> Images { get; set; }
        }
    }
}