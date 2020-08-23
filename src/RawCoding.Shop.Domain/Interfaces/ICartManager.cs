using System.Collections.Generic;
using System.Threading.Tasks;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Domain.Interfaces
{
    public interface ICartManager
    {
        Task<int> UpdateCart(IList<CartProduct> cartProducts);
        void RemoveProduct(int stockId, int qty);
        IList<CartProduct> GetCart(string cartId);
        IEnumerable<CartProduct> GetCartWithStockAndProducts(string cartId);
        void ClearCart();

        void AddCustomerInformation(CustomerInformation customer);
        CustomerInformation GetCustomerInformation();
    }
}
