using System;
using Xamarin.Bookshelf.Mobile.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Xamarin.Bookshelf.Mobile
{
    public class ViewModelLocator
    {        
        public BookshelvesPageViewModel BookshelvesPageViewModel => Startup.GetService<BookshelvesPageViewModel>();
        public BookSearchPageViewModel BookSearchPageViewModel => Startup.GetService<BookSearchPageViewModel>();
        public BookDetailsPageViewModel BookDetailsPageViewModel => Startup.GetService<BookDetailsPageViewModel>();
        public ReviewBookPageViewModel ReviewBookPageViewModel => Startup.GetService<ReviewBookPageViewModel>();
        public LoginPageViewModel LoginPageViewModel => Startup.GetService<LoginPageViewModel>();
        public InitializationSegwayPageViewModel InitializationSegwayPageViewModel => Startup.GetService<InitializationSegwayPageViewModel  >();
    }
}
