using RawCoding.Shop.Domain.Interfaces;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Application.Orders
{
    [Service]
    public class GetOrder
    {
        private readonly IOrderManager _orderManager;

        public GetOrder(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        public Order Do(string orderId) => _orderManager.GetOrderById(orderId);
    }
}