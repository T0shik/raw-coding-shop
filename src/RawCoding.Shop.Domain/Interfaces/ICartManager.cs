using System.Collections.Generic;
using System.Threading.Tasks;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Domain.Interfaces
{
    public interface ICartManager
    {
        Task<int> UpdateCart(IList<CartProduct> cartProducts);
        Task<int> RemoveStock(int stockId, string cartId);
        IList<CartProduct> GetCart(string cartId);
        IEnumerable<CartProduct> GetCartWithStockAndProducts(string cartId);
        void ClearCart();

        void AddCustomerInformation(CustomerInformation customer);
        CustomerInformation GetCustomerInformation();
    }
}
