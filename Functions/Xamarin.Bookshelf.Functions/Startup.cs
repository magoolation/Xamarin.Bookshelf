using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using Xamarin.Bookshelf.Functions.GoogleBooks;

[assembly: FunctionsStartup(typeof(Xamarin.Bookshelf.Functions.Startup))]
namespace Xamarin.Bookshelf.Functions
{
    public class Startup : FunctionsStartup
    {
        private const string BASE_URL = "https://www.googleapis.com";

        public override void Configure(IFunctionsHostBuilder builder)
        {
            _ = builder.Services
                .AddRefitClient<IGoogleBooksApi>()
                 .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(BASE_URL);
                });
        }
    }
}
