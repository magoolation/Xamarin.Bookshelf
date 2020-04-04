using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Xamarin.Bookshelf.Mobile.Models
{
    public class AuthenticationToken
    {
        public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTimeOffset? ExpiresIn { get; set; }

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(AccessToken) && ExpiresIn.HasValue && ExpiresIn <= DateTimeOffset.Now;
    }
}