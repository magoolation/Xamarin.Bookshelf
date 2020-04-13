using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Bookshelf.Shared.Models;
using Xamarin.Essentials;

namespace Xamarin.Bookshelf.Mobile.Services
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly IAuthenticationTokenManager authenticationTokenManager;
        private readonly BookService bookService;

        public AuthenticationManager(IAuthenticationTokenManager authenticationTokenManager, BookService bookService)
        {
            this.authenticationTokenManager = authenticationTokenManager;
            this.bookService = bookService;
        }

        public bool IsAuthenticated => this.authenticationTokenManager.Current.IsAuthenticated;

        public async Task LoginWithGoogle()
        {
            var result = await WebAuthenticator.AuthenticateAsync(new Uri(Constants.AUTHENTICATION_URL), new Uri(Constants.DEEP_LINK_SCHEMA));
            await authenticationTokenManager.Current.SetAuthenticationToken(ExtractToken(result.Properties["token"]));
            var token = await GetTokens("google");
            await StoreTokensAsync(token);

            await RefreshAsync();
        }

        private string ExtractToken(string json)
        {
            return JObject.Parse(
                WebUtility.UrlDecode(json)
                )["authenticationToken"].ToString();
        }

        private async Task<AzureAppServiceAuthenticationToken> GetTokens(string providerName)
        {
            var me = await bookService.Endpoint.MeAsync();

            return me.FirstOrDefault(t => t.ProviderName == providerName);
        }

        private async Task StoreTokensAsync(AzureAppServiceAuthenticationToken tokens)
        {
            await authenticationTokenManager.Current.SetAccessToken(tokens.AccessToken);
            await authenticationTokenManager.Current.SetIdToken(tokens.IdToken);
            await authenticationTokenManager.Current.SetExpiresIn(tokens.ExpiresOn);
            await authenticationTokenManager.Current.SetProviderName(tokens.ProviderName);
            await authenticationTokenManager.Current.SetUserId(tokens.UserId);
        }

        public async Task RefreshAsync()
        {
            await authenticationTokenManager.Current.RefreshAsync();
        }

        public void Logout()
        {
            authenticationTokenManager.Current.RemoveAll();
        }

        public Task RefreshGoogleToken()
        {
            return Task.CompletedTask;
        }
    }
}