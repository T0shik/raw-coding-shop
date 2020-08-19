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

        public class Response
        {
            public int Id { get; set; }
            public string OrderRef { get; set; }
            public string Email { get; set; }
        }

        public IEnumerable<Response> Do(int status) =>
            _orderManager.GetOrdersByStatus((OrderStatus)status,
                x => new Response
                {
                    Id = x.Id,
                    OrderRef = x.OrderRef,
                    Email = x.Email
                });
    }
}
