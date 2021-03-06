﻿using System.Collections.Generic;

namespace RawCoding.Shop.Domain.Models
{
    public class Product
    {
        // todo: staged / published
        public int Id { get; set; }
        public string Name { get; set; }
        public string Series { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string StockDescription { get; set; }
        public List<Image> Images { get; set; } = new List<Image>();
        public IList<Stock> Stock { get; set; } = new List<Stock>();
    }
}
