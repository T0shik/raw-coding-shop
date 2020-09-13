using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RawCoding.Shop.UI.Pages.Checkout
{
    public class Success : PageModel
    {
        public void OnGet(string orderId)
        {
            OrderId = orderId;
        }

        public string OrderId { get; set; }
    }
}