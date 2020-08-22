using Microsoft.AspNetCore.Mvc;

namespace RawCoding.Shop.UI.Pages.Shared.Components.Cart
{
    [ViewComponent(Name = "Cart")]
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("Default");
        }
    }
}