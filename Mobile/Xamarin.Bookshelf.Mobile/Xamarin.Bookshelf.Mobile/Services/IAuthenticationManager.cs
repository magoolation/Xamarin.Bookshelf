using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Bookshelf.Mobile.Models;
using Xamarin.Bookshelf.Shared.Models;

namespace Xamarin.Bookshelf.Mobile.Services
{
    public interface IAuthenticationManager
    {
        Task LoginWithGoogle();
        Task RefreshGoogleToken();

        void Logout();

        Task RefreshAsync();

        bool IsAuthenticated { get; }

        Task SigninWithAppleAsync();
    }
}
