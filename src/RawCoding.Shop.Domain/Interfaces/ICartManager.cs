using System.Collections.Generic;
using System.Threading.Tasks;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Domain.Interfaces
{
    public interface ICartManager
    {
        Task<int> UpdateCart(Cart cart);
        Task<int> RemoveStock(int stockId, string userId);
        Task<int> GetCartId(string userId);
        Task<Cart> GetCartById(int cartId);
        Task<Cart> GetCartByUserId(string userId);
        Task<Cart> GetCartWithStock(string userId);
        IList<CartProduct> GetCartProducts(int cartId);
    }
}
