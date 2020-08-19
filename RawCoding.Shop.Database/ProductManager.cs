using Microsoft.EntityFrameworkCore;
using RawCoding.Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public TResult GetProductById<TResult>(int id, Expression<Func<Product, TResult>> selector)
        {
            return _ctx.Products
                .Where(x => x.Id == id)
                .Select(selector)
                .FirstOrDefault();
        }

        public TResult GetProductByName<TResult>(string name,
            Expression<Func<Product, TResult>> selector)
        {
            return _ctx.Products
                .Include(x => x.Stock)
                .Where(x => x.Name == name)
                .Select(selector)
                .FirstOrDefault();
        }

        public IEnumerable<TResult> GetProducts<TResult>(Func<Product, TResult> selector)
        {
            return _ctx.Products
                .Include(x => x.Stock)
                .Include(x => x.Images)
                .ToList()
                .Select(selector);
        }
    }
}