using System.Collections.Generic;
using RawCoding.Shop.Domain.Enums;
using RawCoding.Shop.Domain.Interfaces;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Application.Admin.Orders
{
    [Service]
    public class GetOrders
    {
        private readonly IOrderManager _orderManager;

        public GetOrders(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        public IEnumerable<object> ForStatus(OrderStatus status) =>
            _orderManager.OrdersForAdminPanel(status);
    }
}