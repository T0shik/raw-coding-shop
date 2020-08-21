using Microsoft.EntityFrameworkCore;
using RawCoding.Shop.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RawCoding.Shop.Domain.Interfaces;

namespace RawCoding.Shop.Database
{
    public class ProductManager : IProductManager
    {
        private readonly ApplicationDbContext _ctx;

        public ProductManager(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public Task<int> CreateProduct(Product product)
        {
            _ctx.Products.Add(product);

            return _ctx.SaveChangesAsync();
        }

        public Task<int> DeleteProduct(int id)
        {
            var product = _ctx.Products.FirstOrDefault(x => x.Id == id);
            _ctx.Products.Remove(product);

            return _ctx.SaveChangesAsync();
        }

        public Task<int> UpdateProduct(Product product)
        {
            _ctx.Products.Update(product);

            return _ctx.SaveChangesAsync();
        }

        public Product GetProductById(int id)
        {
            return _ctx.Products
                .FirstOrDefault(x => x.Id == id);
        }

        public Product GetProductBySlug(string slug)
        {
            return _ctx.Products
                .Include(x => x.Stock)
                .Include(x => x.Images)
                .FirstOrDefault(x => x.Slug == slug);
        }

        public IEnumerable<Product> GetProducts()
        {
            return _ctx.Products
                .Include(x => x.Stock)
                .Include(x => x.Images)
                .ToList();
        }
    }
}