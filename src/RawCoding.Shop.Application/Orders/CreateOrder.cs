using System;
using System.Threading.Tasks;
using RawCoding.Shop.Application.Emails;
using RawCoding.Shop.Domain.Interfaces;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Application.Orders
{
    [Service]
    public class CreateOrder
    {
        private readonly IOrderManager _orderManager;
        private readonly ICartManager _cartManager;
        private readonly IEmailSink _emailSink;
        private readonly IEmailTemplateFactory _emailTemplateFactory;

        public CreateOrder(
            IOrderManager orderManager,
            ICartManager cartManager,
            IEmailSink emailSink,
            IEmailTemplateFactory emailTemplateFactory)
        {
            _orderManager = orderManager;
            _cartManager = cartManager;
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
                Subject = $"Raw Coding - Purchase Reference - {fullOrder.Id}",
                Message = emailMessage,
                Html = true,
            });

            // todo stock manager should extract the order here.
            await _cartManager.Close(order.CartId);
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