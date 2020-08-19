using System.Collections.Generic;
using RawCoding.Shop.Domain.Enums;

namespace RawCoding.Shop.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderRef { get; set; }
        public string StripeReference { get; set; }

        public string FirstName { get; set; }
        public string LastName{ get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }

        public OrderStatus Status { get; set; }

        public IList<OrderStock> OrderStocks { get; set; } = new List<OrderStock>();
    }
}
