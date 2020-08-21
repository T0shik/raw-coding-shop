using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RawCoding.Shop.Application.Admin.Stocks;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.UI.Controllers.Admin
{
    public class StocksController : AdminBaseController
    {
        [HttpGet("")]
        public IActionResult GetStock([FromServices] GetStock getStock) =>
            Ok(getStock.Do());

        [HttpPost("")]
        public Task<object> CreateStock(
            [FromBody] CreateStock.Form request,
            [FromServices] CreateStock createStock) =>
            createStock.Do(request);

        [HttpDelete("{id}")]
        public Task<int> DeleteStock(int id, [FromServices] DeleteStock deleteStock) =>
            deleteStock.Do(id);

        [HttpPut("")]
        public Task<IEnumerable<object>> UpdateStock([FromBody] IEnumerable<Stock> stocks,
            [FromServices] UpdateStock updateStock) =>
            updateStock.Do(stocks);
    }
}