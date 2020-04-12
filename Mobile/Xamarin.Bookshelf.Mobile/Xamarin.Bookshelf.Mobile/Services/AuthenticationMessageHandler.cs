using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Bookshelf.Mobile.Services
{
    public class AuthenticationMessageHandler: DelegatingHandler
    {
        private readonly IAuthenticationManager authenticationManager;

        public AuthenticationMessageHandler(IAuthenticationManager authenticationManager)
        {
            this.authenticationManager = authenticationManager;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization == null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authenticationManager.Current.AccessToken);
            }
            request.Headers.Add("X-ZUMO-AUTH", authenticationManager.Current.AccessToken);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
