using System;
using System.Collections.Generic;

namespace RawCoding.Shop.Domain.Models
{
    public class Cart
    {
        public string Id { get; set; }
        public bool Closed { get; set; }
        public DateTimeOffset LastUpdated { get; set; } = DateTimeOffset.UtcNow;
        public IList<CartProduct> Products { get; set; } = new List<CartProduct>();
    }
}