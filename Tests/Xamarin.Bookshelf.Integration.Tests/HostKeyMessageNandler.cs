using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Bookshelf.Integration.Tests
{
    public class HostKeyMessageNandler : HttpClientHandler
        {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = Environment.GetEnvironmentVariable("INTEGRATION_TESTS_API_KEY");
            var builder = new UriBuilder(request.RequestUri);

            if (string.IsNullOrWhiteSpace(builder.Query))
                    {
                builder.Query = $"{builder.Query}?{token}";
            }
            else
            {
                builder.Query = $"{builder.Query}&{token}";
            }
            
            request.RequestUri = builder.Uri;

            return base.SendAsync(request, cancellationToken);
        }
    }
}