using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Bookshelf.Functions.GoogleBooks;

[assembly: FunctionsStartup(typeof(Xamarin.Bookshelf.Functions.Startup))]
namespace Xamarin.Bookshelf.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<GoogleBooksApiService>();
        }
    }
}
