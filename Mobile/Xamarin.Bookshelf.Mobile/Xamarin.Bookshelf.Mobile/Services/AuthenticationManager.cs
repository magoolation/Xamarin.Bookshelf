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
            var result = await WebAuthenticator.AuthenticateAsync(new Uri(Constants.AUTHENTICATION_URL_GOOGLE), new Uri(Constants.DEEP_LINK_SCHEMA));
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

        public async Task SigninWithAppleAsync()
        {
            WebAuthenticatorResult result = null;
            if (DeviceInfo.Platform == DevicePlatform.iOS
        && DeviceInfo.Version.Major >= 13)
            {
                result = await AppleSignInAuthenticator.AuthenticateAsync(new AppleSignInAuthenticator.Options() { IncludeEmailScope = true, IncludeFullNameScope = true });
            }
            else
            {
                await WebAuthenticator.AuthenticateAsync(new Uri(Constants.AUTHENTICATION_URL_APPLE), new Uri(Constants.DEEP_LINK_SCHEMA));
            }

            if (result != null)
                {
                await authenticationTokenManager.Current.SetUserId(result.Properties["user_id"]);
                await authenticationTokenManager.Current.SetAccessToken(result.AccessToken);
                await authenticationTokenManager.Current.SetIdToken(result.IdToken);

                var apple = new AppleAuthenticationResult()
                {
                    UserId = result.Properties["user_id"],
                    Name = result.Properties["name"],
                    Email = result.Properties["email"],
                    AccessToken = result.AccessToken,
                    ExpiresIn = result.ExpiresIn,
                    RefreshToken = result.RefreshToken
                };
                var token = await bookService.Endpoint.SigninWithAppleAsync(apple);

                await authenticationTokenManager.Current.SetAuthenticationToken(token);
            }
        }
    }
} 