using System;

namespace Xamarin.Bookshelf.Shared.Models
{
    public class AppleAuthenticationResult
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string AccessToken { get; set; }
        public DateTimeOffset? ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
    }
}
