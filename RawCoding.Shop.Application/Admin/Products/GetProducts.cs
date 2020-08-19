using System.Collections.Generic;
using System.Linq;
using RawCoding.Shop.Domain.Interfaces;

namespace RawCoding.Shop.Application.Admin.Products
{
    [Service]
    public class GetProducts
    {
        private readonly IProductManager _productManager;

        public GetProducts(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public IEnumerable<object> Do() =>
            _productManager.GetProducts(x => new
            {
                x.Id,
                x.Name,
                x.Value,

                Images = x.Images.Select(img => new
                {
                    img.Path,
                    img.Index,
                })
            });
    }
}
