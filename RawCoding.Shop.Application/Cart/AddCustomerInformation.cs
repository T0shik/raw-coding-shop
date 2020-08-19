using RawCoding.Shop.Domain.Interfaces;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Application.Cart
{
    [Service]
    public class AddCustomerInformation
    {
        private readonly ICartManager _cartManager;

        public AddCustomerInformation(ICartManager cartManager)
        {
            _cartManager = cartManager;
        }

        public class Form
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }

            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string PostCode { get; set; }
        }

        public void Do(Form request)
        {
            _cartManager.AddCustomerInformation(new CustomerInformation
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address1 = request.Address1,
                Address2 = request.Address2,
                City = request.City,
                PostCode = request.PostCode,
            });
        }
    }
}
