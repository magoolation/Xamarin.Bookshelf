using System;
using System.Dynamic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Xamarin.Bookshelf.Mobile.Models
{
    public class TokenStore
    {
        private string userId;
        public string UserId
        {
            get => userId;
            set => Set(ref userId, Constants.USER_ID, value);
        }

        private string accessToken;
        public string AccessToken
        {
            get => accessToken;
            private set => Set(ref accessToken, Constants.ACCESS_TOKEN, value);
        }

        private void Set<T>(ref T field, string key, T value)
        {
            if (!object.Equals(field, value))
            {
                field = value;
                if (value == null)
                {
                    SecureStorage.Remove(key);
                }
            }
        }

        private string idToken;
        public string IdToken
        {
            get => idToken;
            set => Set(ref idToken, Constants.ID_TOKEN, value);
        }
        private DateTimeOffset? expiresIn;
        public DateTimeOffset? ExpiresIn
        {
            get => expiresIn;
            set => Set(ref expiresIn, Constants.EXPIRES_IN, value);
        }

        private string providerName;
        public string ProviderName
        {
            get => providerName;
            set => Set(ref providerName, Constants.PROVIDER_NAME, value);
        }

        public bool IsAuthenticated => !string.IsNullOrWhiteSpace(AuthenticationToken);

        private string authenticationToken;
        public string AuthenticationToken
        {
            get => authenticationToken;
            set=> Set(ref authenticationToken, Constants.AUTHENTICATION_TOKEN, value);
            }

        public async Task SetAuthenticationToken(string value)
        {
            if (value != AuthenticationToken)
                {
            AuthenticationToken = value;
                if (!string.IsNullOrWhiteSpace(value))
                    {
                    await SecureStorage.SetAsync(Constants.AUTHENTICATION_TOKEN, value);
                }
            }
        }

        public async Task SetAccessToken(string value)
        {
            if (value != accessToken)
                {
                AccessToken = value;
                if (!string.IsNullOrWhiteSpace(value))
                    {
                    await SecureStorage.SetAsync(Constants.ACCESS_TOKEN, value);
                }
            }
        }

        internal void RemoveAll()
        {
            SecureStorage.RemoveAll();
        }

        public async  Task RefreshAsync()
        {
            UserId = await SecureStorage.GetAsync(Constants.USER_ID);
            AuthenticationToken = await SecureStorage.GetAsync(Constants.AUTHENTICATION_TOKEN);
            AccessToken = await SecureStorage.GetAsync(Constants.ACCESS_TOKEN);
            IdToken = await SecureStorage.GetAsync(Constants.ID_TOKEN);
            if (DateTimeOffset.TryParse(await SecureStorage.GetAsync(Constants.EXPIRES_IN), out var expiresIn))
            {
                ExpiresIn = expiresIn;
            }
        }

        public async Task SetIdToken(string value)
        {
            if (value != idToken)
                {
                IdToken = value;
                if (!string.IsNullOrWhiteSpace(value))
                    {
                    await SecureStorage.SetAsync(Constants.ID_TOKEN, value);
                }
            }
        }

        public async Task SetProviderName(string value)
        {
            if (value != providerName)
            {
                ProviderName = value;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    await SecureStorage.SetAsync(Constants.PROVIDER_NAME, value);
                }
            }
        }

        public async Task SetUserId(string value)
        {
            if (value != userId)
            {
                UserId = value;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    await SecureStorage.SetAsync(Constants.USER_ID, value);
                }
            }
        }

        public async Task SetExpiresIn(DateTimeOffset? value)
        {
            if (value != expiresIn)
                {
                ExpiresIn = value;
                if (value.HasValue)
                    {
                    await SecureStorage.SetAsync(Constants.EXPIRES_IN, value.Value.ToString());
                }
            }
        }
    }
}