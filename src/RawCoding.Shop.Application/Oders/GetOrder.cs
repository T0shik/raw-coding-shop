using System.Linq;
using RawCoding.Shop.Domain.Extensions;
using RawCoding.Shop.Domain.Interfaces;

namespace RawCoding.Shop.Application.Oders
{
    [Service]
    public class GetOrder
    {
        private readonly IOrderManager _orderManager;

        public GetOrder(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        public object Do(string reference) =>
            _orderManager.GetOrderByReference(reference, (order) =>
                new
                {
                    order.OrderRef,
                    order.FirstName,
                    order.LastName,
                    order.Email,
                    order.PhoneNumber,
                    order.Address1,
                    order.Address2,
                    order.City,
                    order.PostCode,

                    Products = order.OrderStocks.Select(y => new
                    {
                        y.Stock.Product.Name,
                        y.Stock.Product.Description,
                        Value = $"£ {y.Stock.Value.ToMoney()}",
                        y.Qty,
                        StockDescription = y.Stock.Product.StockDescription,
                    }),

                    TotalValue = order.OrderStocks.Sum(y => y.Stock.Value).ToMoney()
                });
    }
}