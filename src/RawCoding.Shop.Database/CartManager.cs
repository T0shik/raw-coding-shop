using System;
using System.Collections.Generic;
using RawCoding.Shop.Domain.Interfaces;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Database
{
    public class CartManager : ICartManager
    {
        public void AddProduct(CartProduct cartProduct)
        {
            throw new NotImplementedException();
        }

        public void RemoveProduct(int stockId, int qty)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TResult> GetCart<TResult>(Func<CartProduct, TResult> selector)
        {
            throw new NotImplementedException();
        }

        public void ClearCart()
        {
            throw new NotImplementedException();
        }

        public void AddCustomerInformation(CustomerInformation customer)
        {
            throw new NotImplementedException();
        }

        public CustomerInformation GetCustomerInformation()
        {
            throw new NotImplementedException();
        }
    }
}
