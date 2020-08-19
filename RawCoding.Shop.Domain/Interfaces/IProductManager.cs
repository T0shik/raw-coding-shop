using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Domain.Interfaces
{
    public interface IProductManager
    {
        Task<int> CreateProduct(Product product);
        Task<int> DeleteProduct(int id);
        Task<int> UpdateProduct(Product product);

        TResult GetProductById<TResult>(int id, Expression<Func<Product, TResult>> selector);
        TResult GetProductByName<TResult>(string name, Expression<Func<Product, TResult>> selector);
        IEnumerable<TResult> GetProducts<TResult>(Func<Product, TResult> selector);
    }
}
