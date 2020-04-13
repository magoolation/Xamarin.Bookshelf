using Xamarin.Bookshelf.Mobile.Models;

namespace Xamarin.Bookshelf.Mobile.Services
{
    public interface IAuthenticationTokenManager
    {
        TokenStore Current { get; }
    }
}