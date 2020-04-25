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
        private readonly IAuthenticationTokenManager authenticationTokenManager;

        public AuthenticationMessageHandler(IAuthenticationTokenManager authenticationTokenManager)
        {
            this.authenticationTokenManager = authenticationTokenManager;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (authenticationTokenManager.Current.IsAuthenticated)
            {
                //    if (request.Headers.Authorization == null)
                //    {
                //        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authenticationTokenManager.Current.AuthenticationToken);
                //    }
                request.Headers.Add("X-ZUMO-AUTH", authenticationTokenManager.Current.AuthenticationToken);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
