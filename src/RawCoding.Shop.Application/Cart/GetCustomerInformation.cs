using RawCoding.Shop.Domain.Interfaces;

namespace RawCoding.Shop.Application.Cart
{
    [Service]
    public class GetCustomerInformation
    {
        private readonly ICartManager _cartManager;

        public GetCustomerInformation(ICartManager cartManager)
        {
            _cartManager = cartManager;
        }

        public object Do()
        {
            var customerInformation = _cartManager.GetCustomerInformation();

            if (customerInformation == null)
                return null;

            return new
            {
                customerInformation.FirstName,
                customerInformation.LastName,
                customerInformation.Email,
                customerInformation.PhoneNumber,
                customerInformation.Address1,
                customerInformation.Address2,
                customerInformation.City,
                customerInformation.PostCode,
            };
        }
    }
}