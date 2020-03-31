using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Bookshelf.Shared.Models;
using ABookshelf = Xamarin.Bookshelf.Shared.Models.Bookshelf;

namespace Xamarin.Bookshelf.Shared.Services
{
    [Headers("x-functions-key: jTLTHtsS7iWfDUqXpPfVVfRHdwKUoUnUDyV6UCUcbb7e/658sGkz3Q==")]
    public interface IBookService
    {
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
        Task RegisterBookAsync([Body]ABookshelf bookshelf);
            [Post("/v1/reviews")]
        Task ReviewBookAsync([Body]BookReview review);
    }
}
