using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.Bookshelf.Mobile.ViewModels;
using Xamarin.Bookshelf.Mobile.Views;
using Xamarin.Bookshelf.Shared;
using Xamarin.Bookshelf.Shared.Services;

namespace Xamarin.Bookshelf.Mobile
{
    public class ViewModelLocator
    {
        private readonly Lazy<IBookService> bookService = new Lazy<IBookService>(() => RestService.For<IBookService>(ApiRoutes.API_BASE_URL));

        public BookshelvesPageViewModel BookshelvesPageViewModel { get; }
        public BookSearchPageViewModel BookSearchPageViewModel { get; }

        public ViewModelLocator()
        {
            BookshelvesPageViewModel = new BookshelvesPageViewModel(bookService.Value);
            BookSearchPageViewModel = new BookSearchPageViewModel(bookService.Value);
        }
    }
}
