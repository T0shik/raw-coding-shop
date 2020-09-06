using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RawCoding.Shop.Application.Admin.Orders;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.UI.Pages.Orders
{
    public class OrderPage : PageModel
    {
        public Order Order { get; set; }

        public IActionResult OnGet(string id,
            [FromServices] GetOrder getOrder)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToPage("/Index");
            }

            Order = getOrder.Do(id);

            if (Order == null)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }
    }
}