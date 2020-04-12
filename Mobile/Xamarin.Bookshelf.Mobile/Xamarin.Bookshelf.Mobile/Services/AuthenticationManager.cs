using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Bookshelf.Mobile.Models;
using Xamarin.Essentials;

namespace Xamarin.Bookshelf.Mobile.Services
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private AuthenticationToken current;
        public AuthenticationToken Current => current;

        public async Task LoginWithGoogle()
        {
            var result = await WebAuthenticator.AuthenticateAsync(new Uri(Constants.AUTHENTICATION_URL), new Uri(Constants.DEEP_LINK_SCHEMA));
            await StoreTokensAsync(result);
            await RefreshAsync();
        }

        private static async Task StoreTokensAsync(WebAuthenticatorResult result)
        {
            if (!string.IsNullOrWhiteSpace(result.AccessToken))
            {
                await SecureStorage.SetAsync(Constants.ACCESS_TOKEN, result.AccessToken); 
            }
            else if (!string.IsNullOrWhiteSpace(result.Properties["token"]))
            {
                var query = WebUtility.UrlDecode(result.Properties["token"]);
                var json = JObject.Parse(query);
                var token = json["authenticationToken"].ToString();
                await SecureStorage.SetAsync(Constants.ACCESS_TOKEN, token);
            }

            if (!string.IsNullOrWhiteSpace(result.RefreshToken))
            {
                await SecureStorage.SetAsync(Constants.REFRESH_TOKEN, result.RefreshToken); 
            }

            if (result.ExpiresIn.HasValue)
            {
                await SecureStorage.SetAsync(Constants.EXPIRES_IN, result.ExpiresIn.ToString()); 
            }
        }

        public async Task RefreshAsync()
        {
            current = new AuthenticationToken();
            current.AccessToken = await SecureStorage.GetAsync(Constants.ACCESS_TOKEN);
            current.RefreshToken = await SecureStorage.GetAsync(Constants.REFRESH_TOKEN);
            current.ExpiresIn = DateTimeOffset.Parse(await SecureStorage.GetAsync(Constants.EXPIRES_IN));
        }        

        public void Logout()
        {
            SecureStorage.RemoveAll();
        }

        public Task RefreshGoogleToken()
        {
            return Task.CompletedTask;
        }
    }


}
