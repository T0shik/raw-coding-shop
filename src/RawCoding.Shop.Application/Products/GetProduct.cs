using RawCoding.Shop.Domain.Interfaces;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Application.Products
{
    [Service]
    public class GetProduct
    {
        private readonly IProductManager _productManager;

        public GetProduct(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public Product Do(string slug)
        {
            return _productManager.GetProductBySlug(slug);
        }
    }
}