using System;
using System.Collections.Generic;
using Refit;
using System.Net.Http;
using System.Text;
using Xamarin.Bookshelf.Shared.Services;

namespace Xamarin.Bookshelf.Mobile.Services
{
    public class BookService
    {
        private readonly IBookService bookService;

        public BookService(HttpClient client)
        {
bookService = RestService.For<IBookService>(client);
        }

        public IBookService Endpoint => bookService;
    }
}
