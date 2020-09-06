using System.Collections.Generic;
using System.Threading.Tasks;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Domain.Interfaces
{
    public interface ICartManager
    {
        Task<Cart> CreateCart(string cartId);
        Task<int> UpdateCart(Cart cartProducts);
        Task<int> RemoveStock(int stockId, string cartId);
        Cart GetCart(string cartId);
        IList<CartProduct> GetCartProducts(string cartId);
        void ClearCart();

        void AddCustomerInformation(CustomerInformation customer);
        CustomerInformation GetCustomerInformation();
    }
}
