using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace RawCoding.Shop.UI.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUserId(this ClaimsPrincipal @this) => @this.Claims?
            .FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
    }
}