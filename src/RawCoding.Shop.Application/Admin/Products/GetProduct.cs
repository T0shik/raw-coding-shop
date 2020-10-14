using RawCoding.Shop.Domain.Interfaces;
using RawCoding.Shop.Domain.Models;

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

        public Product Do(int id) =>
            _productManager.GetAdminPanelProduct(id);
    }
}