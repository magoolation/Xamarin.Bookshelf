using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.Bookshelf.Mobile.Services;
using Xamarin.Bookshelf.Mobile.ViewModels;
using Xamarin.Bookshelf.Shared;
using Xamarin.Bookshelf.Shared.Services;

namespace Xamarin.Bookshelf.Mobile
{
    public class ViewModelLocator
    {
        private readonly Lazy<IBookService> bookService = new Lazy<IBookService>(() => RestService.For<IBookService>(ApiRoutes.API_BASE_URL));
        private readonly Lazy<IBookRepository> bookRepository = new Lazy<IBookRepository>(() => new BookRepository());

        public BookshelvesPageViewModel BookshelvesPageViewModel { get; }
        public BookSearchPageViewModel BookSearchPageViewModel { get; }
        public BookDetailsPageViewModel BookDetailsPageViewModel { get;  }
        public ReviewBookPageViewModel ReviewBookPageViewModel { get; }
        public LoginPageViewModel LoginPageViewModel { get; }
        public InitializationSegwayPageViewModel InitializationSegwayPageViewModel { get; }

        public ViewModelLocator()
        {
            BookshelvesPageViewModel = new BookshelvesPageViewModel(bookService.Value, bookRepository.Value);
            BookSearchPageViewModel = new BookSearchPageViewModel(bookService.Value, bookRepository.Value);
            BookDetailsPageViewModel = new BookDetailsPageViewModel(bookService.Value, bookRepository.Value);
            ReviewBookPageViewModel = new ReviewBookPageViewModel(bookService.Value);
            LoginPageViewModel = new LoginPageViewModel();
            InitializationSegwayPageViewModel = new InitializationSegwayPageViewModel();
        }
    }
}
