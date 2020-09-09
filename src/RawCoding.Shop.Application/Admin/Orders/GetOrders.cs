using System.Collections.Generic;
using RawCoding.Shop.Domain.Enums;
using RawCoding.Shop.Domain.Interfaces;

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

        public IEnumerable<object> Do(int status) =>
            _orderManager.GetOrdersByStatus((OrderStatus)status,
                x => new
                {
                    x.Id,
                    OrderRef = x.Id,
                    x.Cart.Email
                });
    }
}
