using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;
using System;
using System.Reflection;
using System.Threading.Tasks;
using TinyNavigationHelper;
using TinyNavigationHelper.Forms;
using Xamarin.Bookshelf.Mobile.Helpers;
using Xamarin.Bookshelf.Mobile.Services;
using Xamarin.Bookshelf.Mobile.ViewModels;
using Xamarin.Bookshelf.Shared.Services;
using Xamarin.Essentials;

namespace Xamarin.Bookshelf.Mobile
{
    public static class Startup
    {
        private static IHost _host;

        public static IServiceProvider ServiceProvider => _host.Services;

        public static void Init(IPlatformInitializer platformInitializer)
        {
            var a = Assembly.GetExecutingAssembly();
            var stream = a.GetManifestResourceStream("Xamarin.Bookshelf.Mobile.appsettings.json");

            _host = new HostBuilder()
                        .ConfigureHostConfiguration(c =>
                        {
                            c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });
                            //c.AddJsonStream(stream);
                        })
                        .ConfigureServices((c, x) =>
                        {
                            ConfigureServices(c, x, platformInitializer);
                        })
                        .Build();
        }

        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services, IPlatformInitializer platformInitializer)
        {
            // TODO WTS: Register your services, viewmodels and pages here
            services.AddSingleton<IAuthenticationTokenManager, AuthenticationTokenManager>();
            services.AddTransient < AuthenticationMessageHandler>();
            
            services.AddRefitClient<IBookService>()
                .AddHttpMessageHandler<AuthenticationMessageHandler>()
                .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri("https://tracinha-functions.azurewebsites.net/");
            } );

            // Services            
            services.AddSingleton<IAuthenticationManager, AuthenticationManager>();
            services.AddSingleton<INavigationService, NavigationService>();            
            services.AddSingleton<IBookRepository, BookRepository>();

            // ViewModels and Views
            services.AddTransient<BookshelvesPageViewModel>();
            services.AddTransient<BookSearchPageViewModel>();
            services.AddTransient<BookDetailsPageViewModel>();
            services.AddTransient<ReviewBookPageViewModel>();
            services.AddTransient<LoginPageViewModel>();
            services.AddTransient<InitializationSegwayPageViewModel>();

            ConfigureNavigation(services);

            // Configuration
            //services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));

            // Platform specific configuration
            platformInitializer?.ConfigureServices(context, services);            
        }

        private static void ConfigureNavigation(IServiceCollection services)
        {
            var navigationHelper = new ShellNavigationHelper();
            var currentAssembly = Assembly.GetExecutingAssembly();
            navigationHelper.RegisterViewsInAssembly(currentAssembly);

            services.AddSingleton<INavigationHelper>(navigationHelper);

        }


        public static T GetService<T>() where T : class
            => _host.Services.GetService<T>() as T;
    }
}
