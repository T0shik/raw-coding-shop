using System.Collections.Generic;
using RawCoding.Shop.Domain.Enums;

namespace RawCoding.Shop.Domain.Models
{
    public class Order
    {
        public string Id { get; set; }
        public string StripeReference { get; set; }

        public string CartId { get; set; }
        public Cart Cart { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string State { get; set; }

        public OrderStatus Status { get; set; }
    }
}