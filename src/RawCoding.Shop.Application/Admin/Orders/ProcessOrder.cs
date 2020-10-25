using System.Threading.Tasks;
using RawCoding.Shop.Application.Emails;
using RawCoding.Shop.Application.Projections;
using RawCoding.Shop.Domain.Enums;
using RawCoding.Shop.Domain.Interfaces;

namespace RawCoding.Shop.Application.Admin.Orders
{
    [Service]
    public class ProcessOrder
    {
        private readonly IOrderManager _orderManager;
        private readonly IEmailSink _emailSink;
        private readonly IEmailTemplateFactory _emailTemplateFactory;

        public ProcessOrder(
            IOrderManager orderManager,
            IEmailSink emailSink,
            IEmailTemplateFactory emailTemplateFactory)
        {
            _orderManager = orderManager;
            _emailSink = emailSink;
            _emailTemplateFactory = emailTemplateFactory;
        }

        public async Task Ship(string id)
        {
            var order = _orderManager.GetOrderById(id);
            order.Status = OrderStatus.Shipped;

            await _orderManager.UpdateOrder(order);
            var orderConfirmationMessage = await _emailTemplateFactory.RenderShippingConfirmationAsync(order);
            await _emailSink.SendAsync(new SendEmailRequest
            {
                To = order.Cart.Email,
                Subject = $"Raw Coding - Order Shipped - {order.Id}",
                Message = orderConfirmationMessage,
            });
        }
    }
}