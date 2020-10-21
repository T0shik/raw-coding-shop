using RawCoding.Shop.Application.Emails;
using RawCoding.Shop.Application.Projections;
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

        public Order ForUserById(string orderId) => _orderManager.GetOrderById(orderId);

        public object ForAdminById(string orderId)
        {
            var order = _orderManager.GetOrderById(orderId);

            return OrderProjections.Project(order);
        }
    }
}