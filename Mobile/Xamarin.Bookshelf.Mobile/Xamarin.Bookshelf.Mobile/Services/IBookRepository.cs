using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Bookshelf.Shared.Models;

namespace Xamarin.Bookshelf.Mobile.Services
{
    public interface IBookRepository
    {
        Task<BookshelfItemDetails> GetByIdAsync(string id);
        Task<IEnumerable<BookshelfItemDetails>> GetAllBooksAsync();
        Task UpdateBookItemsAsync(BookshelfItemDetails[] items);
        Task AddBookAsync(BookshelfItemDetails bookshelfItem);
        Task<IEnumerable<BookshelfItemDetails>> GetBooksByBookshelf(ReadingStatus status);
    }
}