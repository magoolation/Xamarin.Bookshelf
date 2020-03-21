using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Bookshelf.Shared.Models;

namespace Xamarin.Bookshelf.Shared.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> SearchBookByTitleAsync(string title);
        Task<IEnumerable<Book>> SearchBookByAuthorAsync(string title);
        Task<Book> GetBookDetails(string bookId);
    }
}
