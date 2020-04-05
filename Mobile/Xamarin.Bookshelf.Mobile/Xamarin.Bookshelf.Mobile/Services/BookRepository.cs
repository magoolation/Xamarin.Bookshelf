using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Bookshelf.Shared.Models;
using Xamarin.Essentials;

namespace Xamarin.Bookshelf.Mobile.Services
{
    public class BookRepository : IBookRepository
    {
        private const string DB_NAME = "Bookshelf.db";
        private const string BOOKSHELVES_COLLECTION = "Bookshelves";

        private readonly ILiteDatabase db;
        private readonly ILiteCollection<BookshelfItem> bookshelvesCollection;

        public BookRepository()
        {
            db = new LiteDatabase(Path.Combine(FileSystem.CacheDirectory, DB_NAME));
            bookshelvesCollection = db.GetCollection<BookshelfItem>(BOOKSHELVES_COLLECTION);
        }

        public Task<BookshelfItem> GetByIdAsync(string id)
        {
            return bookshelvesCollection.FindByIdAsync(id);
        }

        public Task UpdateBookItemsAsync(BookshelfItem[] items)
        {
            return bookshelvesCollection.UpsertAsync(items);
        }

        public Task<IEnumerable<BookshelfItem>> GetAllBooksAsync(string userId)
        {
            return bookshelvesCollection.Query().Where(c => c.UserId == userId).ToListAsync();
        }

        public Task AddBookAsync(BookshelfItem bookshelfItem)
        {
            return bookshelvesCollection.InsertAsync(bookshelfItem);
        }

        public async Task<IEnumerable<BookshelfItem>> GetBooksByBookshelf(ReadingStatus status)
        {
            return bookshelvesCollection.FindAll().Where(c => c.ReadingStatus == status);
        }
    }
}
