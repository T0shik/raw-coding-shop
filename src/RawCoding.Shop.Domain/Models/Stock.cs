using System.Collections.Generic;

namespace RawCoding.Shop.Domain.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public int Qty { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public Product Product { get; set; }

        public IList<OrderStock> OrderStocks { get; set; } = new List<OrderStock>();
    }
}