using System;
using System.Collections.Generic;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Domain.Interfaces
{
    public interface ICartManager
    {
        void AddProduct(CartProduct cartProduct);
        void RemoveProduct(int stockId, int qty);
        IEnumerable<TResult> GetCart<TResult>(Func<CartProduct, TResult> selector);
        void ClearCart();

        void AddCustomerInformation(CustomerInformation customer);
        CustomerInformation GetCustomerInformation();
    }
}
