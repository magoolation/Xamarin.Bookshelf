using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Bookshelf.Shared.Models;

namespace Xamarin.Bookshelf.Mobile.Services
{
    public interface IBookRepository
    {
        Task<BookshelfItem> GetByIdAsync(string id);
        Task<IEnumerable<BookshelfItem>> GetAllBooksAsync(string userId);
        Task UpdateBookItemsAsync(BookshelfItem[] items);
    }
}