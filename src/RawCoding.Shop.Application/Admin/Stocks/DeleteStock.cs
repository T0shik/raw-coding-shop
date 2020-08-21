using System.Threading.Tasks;
using RawCoding.Shop.Domain.Interfaces;

namespace RawCoding.Shop.Application.Admin.Stocks
{
    [Service]
    public class DeleteStock
    {
        private readonly IStockManager _stockManager;

        public DeleteStock(IStockManager stockManager)
        {
            _stockManager = stockManager;
        }

        public Task<int> Do(int id)
        {
            return _stockManager.DeleteStock(id);
        }
    }
}
