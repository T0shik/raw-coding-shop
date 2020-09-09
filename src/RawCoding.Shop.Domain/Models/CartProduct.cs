namespace RawCoding.Shop.Domain.Models
{
    public class CartProduct
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public int StockId { get; set; }
        public Stock Stock { get; set; }
        public int Qty { get; set; }
    }
}