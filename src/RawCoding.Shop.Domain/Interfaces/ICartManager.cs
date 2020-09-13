using System.Collections.Generic;
using System.Threading.Tasks;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Domain.Interfaces
{
    public interface ICartManager
    {
        Task Close(int cartId);
        Task<Cart> CreateCart(string userId);
        Task<int> UpdateCart(Cart cart);
        Task<int> RemoveStock(int stockId, string userId);
        Task<int> GetCartId(string userId);
        Task<bool> Empty(string userId);
        Task<Cart> GetCart(string userId);
        Cart GetCartFull(string userId);
        IList<CartProduct> GetCartProducts(int cartId);
        void ClearCart();

        void AddCustomerInformation(CustomerInformation customer);
        CustomerInformation GetCustomerInformation();
    }
}
