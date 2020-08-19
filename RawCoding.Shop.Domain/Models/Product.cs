using System.Collections.Generic;

namespace RawCoding.Shop.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Collection { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }

        public IList<Image> Images { get; set; } = new List<Image>();
        public IList<Stock> Stock { get; set; } = new List<Stock>();
    }
}
