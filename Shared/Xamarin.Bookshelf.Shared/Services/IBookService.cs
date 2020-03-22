using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Bookshelf.Shared.Models;
using ABookshelf = Xamarin.Bookshelf.Shared.Models.Bookshelf;

namespace Xamarin.Bookshelf.Shared.Services
{
    public interface IBookService
    {
        [Get("")]
        Task<IEnumerable<Book>> SearchBookByTitleAsync(string title);
        [Get("")]
        Task<IEnumerable<Book>> SearchBookByAuthorAsync(string title);
        [Get("")]
        Task<Book> GetBookDetails(string bookId);
        [Get("")]
        Task<IEnumerable<BookReview>> GetBookReviews(string bookId);
            [Get("")]
        Task<IEnumerable<ABookshelf>> GetUserBookShelves(string userId);
        [Post("")]
        Task RegisterBook(ABookshelf bookshelf);
            [Post("")]
        Task ReviewBook(BookReview review);
    }
}
