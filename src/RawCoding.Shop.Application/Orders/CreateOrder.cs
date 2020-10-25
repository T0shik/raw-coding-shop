using System;
using System.Threading.Tasks;
using RawCoding.Shop.Application.Admin.Orders;
using RawCoding.Shop.Application.Emails;
using RawCoding.Shop.Domain.Interfaces;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Application.Orders
{
    [Service]
    public class CreateOrder
    {
        private readonly IOrderManager _orderManager;
        private readonly CartCheckout _cartCheckout;
        private readonly IEmailSink _emailSink;
        private readonly IEmailTemplateFactory _emailTemplateFactory;

        public CreateOrder(
            IOrderManager orderManager,
            CartCheckout cartCheckout,
            IEmailSink emailSink,
            IEmailTemplateFactory emailTemplateFactory)
        {
            _orderManager = orderManager;
            _cartCheckout = cartCheckout;
            _emailSink = emailSink;
            _emailTemplateFactory = emailTemplateFactory;
        }

        public async Task Do(Order order)
        {
            order.Id = CreateOrderReference();
            await _orderManager.CreateOrder(order);

            var fullOrder = _orderManager.GetOrderById(order.Id);
            var emailMessage = await _emailTemplateFactory.RenderOrderConfirmationAsync(fullOrder);
            await _emailSink.SendAsync(new SendEmailRequest
            {
                To = fullOrder.Cart.Email,
                Subject = $"Raw Coding - Order Placed - {fullOrder.Id}",
                Message = emailMessage,
            });

            await _cartCheckout.Complete(order.CartId);
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