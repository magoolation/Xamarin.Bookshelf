﻿namespace Xamarin.Bookshelf.Shared
{
    public class ApiRoutes
    {
        public const string API_BASE_URL = "https://tracinha-functions.azurewebsites.net/";
        public const string API_SIGNIN_WITH_APPLE = ".auth/login/Apple";
        public const string API_BOOKS = "v1/books";
        public const string API_SEARCH_BOOKS_BY_TITLE = API_BOOKS + "?title={title}";
        public const string API_SEARCH_BOOKS_BY_AUTOR = API_BOOKS + "?author={author}";
        public const string API_GET_BOOK_DETAILS = API_BOOKS + "/{id}";
        public const string API_REVIEWS = "v1/reviews";
        public const string API_USER_BOOKSHELVES = "v1/me/bookshelves";
        public const string API_GET_USER_BOOK = API_USER_BOOKSHELVES + "/{id}";
        public const string API_GET_BOOK_REVIEWS = API_REVIEWS + "/{bookId}/";
    }
}
