using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Bookshelf.Shared.Models;

namespace Xamarin.Bookshelf.Mobile.Services
{
    public interface IBookRepository
    {
        Task<BookshelfItem> GetByIdAsync(string id);
        Task<IEnumerable<BookshelfItem>> GetAllBooksAsync(string userId);
        Task UpdateBookItemsAsync(BookshelfItem[] items);
        Task AddBookAsync(BookshelfItem bookshelfItem);
        Task<IEnumerable<BookshelfItem>> GetBooksByBookshelf(ReadingStatus status);
    }
}