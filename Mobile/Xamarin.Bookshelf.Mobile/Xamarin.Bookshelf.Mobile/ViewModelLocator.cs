using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Bookshelf.Mobile.Services;
using Xamarin.Bookshelf.Mobile.ViewModels;
using Xamarin.Bookshelf.Shared;
using Xamarin.Bookshelf.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Xamarin.Bookshelf.Mobile
{
    public class ViewModelLocator
    {
        private static IServiceProvider ServiceProvider;

        public static void Init(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public BookshelvesPageViewModel BookshelvesPageViewModel => ServiceProvider.GetService<BookshelvesPageViewModel>();
        public BookSearchPageViewModel BookSearchPageViewModel => ServiceProvider.GetService<BookSearchPageViewModel>();
        public BookDetailsPageViewModel BookDetailsPageViewModel => ServiceProvider.GetService<BookDetailsPageViewModel>();
        public ReviewBookPageViewModel ReviewBookPageViewModel => ServiceProvider.GetService<ReviewBookPageViewModel>();
        public LoginPageViewModel LoginPageViewModel => ServiceProvider.GetService<LoginPageViewModel>();
        public InitializationSegwayPageViewModel InitializationSegwayPageViewModel => ServiceProvider.GetService<InitializationSegwayPageViewModel  >();
    }
}
