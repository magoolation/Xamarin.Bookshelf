using System.Linq;
using System.Security.Claims;

namespace Xamarin.Bookshelf.Functions
{
    public static class Extensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user?.Claims?.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
        }
    }
}
