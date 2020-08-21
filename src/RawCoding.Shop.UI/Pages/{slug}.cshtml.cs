using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RawCoding.Shop.Application.Products;

namespace RawCoding.Shop.UI.Pages
{
    public class Product : PageModel
    {
        [BindProperty] public Form Input { get; set; }

        public IActionResult OnGet(string slug, [FromServices] GetProduct getProduct)
        {
            SelectedProduct = getProduct.Do(slug);

            if (SelectedProduct == null)
                return RedirectToPage("/not-found");

            return Page();
        }

        public RawCoding.Shop.Domain.Models.Product SelectedProduct { get; set; }

        public class Form
        {
            public int ProductId { get; set; }
            public int StockId { get; set; }
            public int Qty { get; set; }
        }
    }
}