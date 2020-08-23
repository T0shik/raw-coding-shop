namespace RawCoding.Shop.Domain.Models
{
    public class CartProduct
    {
        public int Id { get; set; }
        public string CartId { get; set; }
        public int StockId { get; set; }
        public Stock Stock { get; set; }
        public int Qty { get; set; }
        public bool Complete { get; set; }
    }
}