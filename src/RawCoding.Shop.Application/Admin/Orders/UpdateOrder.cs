using System.Threading.Tasks;
using RawCoding.Shop.Domain.Interfaces;

namespace RawCoding.Shop.Application.Admin.Orders
{
    [Service]
    public class UpdateOrder
    {
        private readonly IOrderManager _orderManager;

        public UpdateOrder(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        public Task<int> DoAsync(string id)
        {
            return _orderManager.AdvanceOrder(id);
        }
    }
}
