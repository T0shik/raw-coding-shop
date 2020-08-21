using RawCoding.Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RawCoding.Shop.Domain.Interfaces;

namespace RawCoding.Shop.Application.Oders
{
    [Service]
    public class CreateOrder
    {
        private readonly IOrderManager _orderManager;
        private readonly IStockManager _stockManager;

        // todo stock manager should extract the order here.
        public CreateOrder(
            IOrderManager orderManager,
            IStockManager stockManager)
        {
            _orderManager = orderManager;
            _stockManager = stockManager;
        }

        public class Form
        {
            public string StripeReference { get; set; }
            public string SessionId { get; set; }

            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }

            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string PostCode { get; set; }

            public List<Stock> Stocks { get; set; }
        }

        public class Stock
        {
            public int StockId { get; set; }
            public int Qty { get; set; }
        }

        public Task Do(Form request)
        {
            var order = new Order
            {
                OrderRef = CreateOrderReference(),
                StripeReference = request.StripeReference,

                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address1 = request.Address1,
                Address2 = request.Address2,
                City = request.City,
                PostCode = request.PostCode,

                OrderStocks = request.Stocks.Select(x => new OrderStock
                {
                    StockId = x.StockId,
                    Qty = x.Qty,
                }).ToList()
            };

            return _orderManager.CreateOrder(order);
        }

        private string CreateOrderReference()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var result = new char[14];
            var random = new Random();

            do
            {
                for (int i = 0; i < result.Length; i++)
                    result[i] = chars[random.Next(chars.Length)];
            } while (_orderManager.OrderReferenceExists(new string(result)));

            return new string(result);
        }
    }
}
