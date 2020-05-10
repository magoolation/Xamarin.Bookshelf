using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Bookshelf.Shared.Models;

namespace Xamarin.Bookshelf.Shared.Services
{
    public interface IBookService
    {
        [Post("/.auth/login/Apple")]
        Task<string> SigninWithAppleAsync([Body]AppleAuthenticationResult appleAuthenticationResult);
        [Get("/.auth/me")]
        Task<AzureAppServiceAuthenticationToken[]> MeAsync();
        [Get("/.auth/refresh")]
        Task<HttpResponseMessage> RefreshAsync();
        [Get("/v1/books?title={title}")]
        Task<IEnumerable<BookSummary>> SearchBookByTitleAsync(string title);
        [Get("/v1/books?author={author}")]
        Task<IEnumerable<BookSummary>> SearchBookByAuthorAsync(string author);
        [Get("/v1/books/{id}")]
        Task<BookDetails> GetBookDetailsAsync(string id);
        [Get("/v1/reviews/{bookId}")]
        Task<IEnumerable<BookshelfItemDetails>> GetBookshelfItemAsync(string bookId);
        [Get("/v1/reviews/{bookId}")]
        Task<IEnumerable<UserBookReview>> GetBookReviewsAsync(string bookId);
        [Get("/v1/bookshelves/{userId}")]
        Task<IEnumerable<UserBookshelf>> GetUserBookShelvesAsync(string userId);
        [Post("/v1/bookshelves")]
        Task RegisterBookAsync([Body]BookRegistration registration);
            [Post("/v1/reviews")]
        Task ReviewBookAsync([Body]BookReviewRegistration review);
    }
}
