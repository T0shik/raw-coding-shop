using System.Collections.Generic;
using RawCoding.Shop.Domain.Enums;

namespace RawCoding.Shop.Domain.Models
{
    public class Order
    {
        public string Id { get; set; }
        public string StripeReference { get; set; }

        public int CartId { get; set; }
        public Cart Cart { get; set; }

        public OrderStatus Status { get; set; }
    }
}