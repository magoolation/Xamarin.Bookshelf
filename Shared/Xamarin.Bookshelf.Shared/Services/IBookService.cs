using Refit;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Bookshelf.Shared.Models;

namespace Xamarin.Bookshelf.Shared.Services
{
    public interface IBookService
    {
        [Get("/.auth/me")]
        Task<AzureAppServiceAuthenticationTokens> MeAsync();
        [Get("/.auth/refresh")]
        Task<HttpResponseMessage> RefreshAsync();
        [Get("/v1/books?title={title}")]
        Task<IEnumerable<Book>> SearchBookByTitleAsync(string title);
        [Get("/v1/books?author={author}")]
        Task<IEnumerable<Book>> SearchBookByAuthorAsync(string author);
        [Get("/v1/books/{id}")]
        Task<Book> GetBookDetailsAsync(string id);
        [Get("/v1/reviews/{bookId}")]
        Task<IEnumerable<BookReview>> GetBookReviewsAsync(string bookId);
            [Get("/v1/bookshelves/{userId}")]
        Task<IEnumerable<UserBookshelf>> GetUserBookShelvesAsync(string userId);
        [Post("/v1/bookshelves")]
        Task RegisterBookAsync([Body]BookshelfItem bookshelf);
            [Post("/v1/reviews")]
        Task ReviewBookAsync([Body]BookReview review);
    }
}
