using System.Linq;
using RawCoding.Shop.Domain.Interfaces;

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

        public object Do(string name)
        {
            return _productManager
                .GetProductByName(name, x => new
                {
                    x.Name,
                    x.Description,
                    x.Value,

                    Stock = x.Stock.Select(y => new
                    {
                        y.Id,
                        y.Description,
                        y.Qty
                    })
                });
        }
    }
}