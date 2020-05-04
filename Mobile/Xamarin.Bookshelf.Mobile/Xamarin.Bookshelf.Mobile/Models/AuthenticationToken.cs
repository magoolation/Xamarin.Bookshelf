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

        public bool IsAuthenticated => !string.IsNullOrWhiteSpace(AuthenticationToken) && (ExpiresIn >= DateTime.UtcNow);

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
                    ExpiresIn = DateTime.UtcNow.AddHours(8);
                    await SecureStorage.SetAsync(Constants.AUTHENTICATION_TOKEN, value);
                    await SecureStorage.SetAsync(Constants.EXPIRES_IN, ExpiresIn.ToString());
                }
            }
        }
        
        public void RemoveAll()
        {
            SecureStorage.RemoveAll();
        }

        public async  Task RefreshAsync()
        {
            UserId = await SecureStorage.GetAsync(Constants.USER_ID).ConfigureAwait(false);
            AuthenticationToken = await SecureStorage.GetAsync(Constants.AUTHENTICATION_TOKEN).ConfigureAwait(false);
            if (DateTimeOffset.TryParse(await SecureStorage.GetAsync(Constants.EXPIRES_IN).ConfigureAwait(false), out var expiresIn))
            {
                ExpiresIn = expiresIn;
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