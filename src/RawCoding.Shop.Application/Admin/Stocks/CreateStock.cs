using System.Threading.Tasks;
using RawCoding.Shop.Domain.Interfaces;

namespace RawCoding.Shop.Application.Admin.Stocks
{
    [Service]
    public class CreateStock
    {
        private readonly IStockManager _stockManager;

        public CreateStock(IStockManager stockManager)
        {
            _stockManager = stockManager;
        }

        public async Task<object> Do(Form request)
        {
            var stock = new Domain.Models.Stock
            {
                Qty = request.Qty,
                ProductId = request.ProductId
            };

            await _stockManager.CreateStock(stock);

            return new
            {
                stock.Id,
                stock.Qty
            };
        }

        public class Form
        {
            public int ProductId { get; set; }
            public string Description { get; set; }
            public int Qty { get; set; }
        }
    }
}