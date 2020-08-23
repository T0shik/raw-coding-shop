using System;
using System.Threading.Tasks;
using RawCoding.Shop.Domain.Interfaces;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Application.Admin.Products
{
    [Service]
    public class CreateProduct
    {
        private readonly IProductManager _productManager;

        public CreateProduct(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public async Task<object> Do(Product request)
        {
            if (await _productManager.CreateProduct(request) <= 0)
            {
                throw new Exception("Failed to create product");
            }

            return new
            {
                request.Id,
                request.Name,
                request.Description,
            };
        }
    }
}