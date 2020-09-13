using System;
using System.Threading.Tasks;
using RawCoding.Shop.Domain.Interfaces;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Application.Orders
{
    [Service]
    public class CreateOrder
    {
        private readonly IOrderManager _orderManager;
        private readonly ICartManager _cartManager;

        public CreateOrder(
            IOrderManager orderManager,
            ICartManager cartManager)
        {
            _orderManager = orderManager;
            _cartManager = cartManager;
        }

        public Task Do(Order order)
        {
            order.Id = CreateOrderReference();
            // todo send email
            // todo stock manager should extract the order here.
            return Task.WhenAll(_cartManager.Close(order.CartId), _orderManager.CreateOrder(order));
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