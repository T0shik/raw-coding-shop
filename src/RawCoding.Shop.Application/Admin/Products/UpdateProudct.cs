using System.Threading.Tasks;
using RawCoding.Shop.Domain.Interfaces;

namespace RawCoding.Shop.Application.Admin.Products
{
    [Service]
    public class UpdateProduct
    {
        private readonly IProductManager _productManager;

        public UpdateProduct(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public async Task<object> Do(Form request)
        {
            var product = _productManager.GetProductById(request.Id);

            product.Name = request.Name;
            product.Description = request.Description;

            await _productManager.UpdateProduct(product);

            return new
            {
                product.Id,
                product.Name,
                product.Description,
            };
        }

        public class Form
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int Value { get; set; }
        }
    }
}