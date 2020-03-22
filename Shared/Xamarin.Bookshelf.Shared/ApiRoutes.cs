using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Bookshelf.Shared
{
    public class ApiRoutes
    {
        public const string API_SEARCH_BOOKS = "v1/books";
        public const string API_GET_BOOK_DETAILS = "v1/books/{id}";
        public const string API_SEND_BOOK_REVIEW = "v1/reviews";
        public const string API_REGISTER_BOOK = "v1/bookshelves";
        public const string API_GET_USER_BOOKS = "v1/bookshelves/{userId}/";
        public const string API_GET_BOOK_REVIEWS = "v1/reviews/{bookId}/";
    }
}
