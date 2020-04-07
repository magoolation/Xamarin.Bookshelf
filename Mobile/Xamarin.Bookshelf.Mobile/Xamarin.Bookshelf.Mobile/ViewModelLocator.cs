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

        public BookshelvesPageViewModel BookshelvesPageViewModel => new BookshelvesPageViewModel(bookService.Value, bookRepository.Value);
        public BookSearchPageViewModel BookSearchPageViewModel => new BookSearchPageViewModel(bookService.Value, bookRepository.Value);
        public BookDetailsPageViewModel BookDetailsPageViewModel => new BookDetailsPageViewModel(bookService.Value, bookRepository.Value);
        public ReviewBookPageViewModel ReviewBookPageViewModel => new ReviewBookPageViewModel(bookService.Value);
        public LoginPageViewModel LoginPageViewModel => new LoginPageViewModel();
        public InitializationSegwayPageViewModel InitializationSegwayPageViewModel => new InitializationSegwayPageViewModel();        
    }
}
