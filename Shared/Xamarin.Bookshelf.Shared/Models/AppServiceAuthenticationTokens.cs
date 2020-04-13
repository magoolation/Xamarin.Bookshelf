using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Bookshelf.Shared.Models
{

    public class AzureAppServiceAuthenticationTokens
    {
        public AzureAppServiceAuthenticationToken[] Tokens { get; set; }
    }

    public class AzureAppServiceAuthenticationToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("expires_on")]
        public DateTimeOffset? ExpiresOn { get; set; }
        [JsonProperty("id_token")]
        public string IdToken { get; set; }
        [JsonProperty("provider_name")]
        public string ProviderName { get; set; }
        [JsonProperty("user_claims")]
        public UserClaim[] UserClaims { get; set; }
        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }

    public class UserClaim
    {
        [JsonProperty("type")]
        public string @Type { get; set; }
        [JsonProperty("val")]
        public string Value { get; set; }
    }
}