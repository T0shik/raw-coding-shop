﻿using System.Collections.Generic;
using System.Threading.Tasks;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Domain.Interfaces
{
    public interface IProductManager
    {
        Task<int> CreateProduct(Product product);
        Task<int> UpdateProduct(Product product);

        Product GetProductBySlug(string slug);
        IEnumerable<Product> GetFrontPageProducts();

        #region Admin

        Task UpdateProductStock(int id, IEnumerable<Stock> stock);
        IEnumerable<Product> GetAdminPanelProducts();
        Product GetAdminPanelProduct(int id);

        #endregion
    }
}