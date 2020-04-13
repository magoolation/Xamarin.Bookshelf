using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Bookshelf.Mobile.Models;

namespace Xamarin.Bookshelf.Mobile.Services
{
    public class AuthenticationTokenManager : IAuthenticationTokenManager
    {
        private TokenStore current = new TokenStore();
        public TokenStore Current => current;
    }
}
