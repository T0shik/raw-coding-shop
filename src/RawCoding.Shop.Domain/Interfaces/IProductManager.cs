using System.Collections.Generic;
using System.Threading.Tasks;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Domain.Interfaces
{
    public interface IProductManager
    {
        Task<int> CreateProduct(Product product);
        Task<int> DeleteProduct(int id);
        Task<int> UpdateProduct(Product product);

        Product GetProductById(int id);
        Product GetProductBySlug(string slug);
        IEnumerable<Product> GetProducts();
    }
}
