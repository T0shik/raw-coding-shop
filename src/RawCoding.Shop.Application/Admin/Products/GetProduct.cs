using RawCoding.Shop.Domain.Interfaces;

namespace RawCoding.Shop.Application.Admin.Products
{
    [Service]
    public class GetProduct
    {
        private readonly IProductManager _productManager;

        public GetProduct(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public object Do(int id) =>
            _productManager.GetProductById(id);
    }
}
