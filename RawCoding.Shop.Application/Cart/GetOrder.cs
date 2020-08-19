using RawCoding.Shop.Domain.Interfaces;

namespace RawCoding.Shop.Application.Cart
{
    [Service]
    public class GetOrder
    {
        private readonly ICartManager _cartManager;

        public GetOrder(ICartManager cartManager)
        {
            _cartManager = cartManager;
        }

        public object Do()
        {
            var listOfProducts = _cartManager
                .GetCart(x => new
                {
                    x.ProductId,
                    x.StockId,
                    Value = (int) (x.Value * 100),
                    x.Qty
                });

            var customerInformation = _cartManager.GetCustomerInformation();

            return new
            {
                Products = listOfProducts,
                CustomerInformation = new
                {
                    customerInformation.FirstName,
                    customerInformation.LastName,
                    customerInformation.Email,
                    customerInformation.PhoneNumber,
                    customerInformation.Address1,
                    customerInformation.Address2,
                    customerInformation.City,
                    customerInformation.PostCode,
                }
            };
        }
    }
}