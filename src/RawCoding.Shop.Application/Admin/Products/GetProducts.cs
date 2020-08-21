using System.Collections.Generic;
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
            _productManager.GetProducts();
    }
}
