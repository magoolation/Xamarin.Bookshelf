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
        private readonly ILiteCollection<BookshelfItemDetails> bookshelvesCollection;

        public BookRepository()
        {
            db = new LiteDatabase(Path.Combine(FileSystem.CacheDirectory, DB_NAME));
            bookshelvesCollection = db.GetCollection<BookshelfItemDetails>(BOOKSHELVES_COLLECTION);
        }

        public Task<BookshelfItemDetails> GetByIdAsync(string id)
        {
            return bookshelvesCollection.FindByIdAsync(id);
        }

        public Task UpdateBookItemsAsync(BookshelfItemDetails[] items)
        {
            return bookshelvesCollection.UpsertAsync(items);
        }

        public Task<IEnumerable<BookshelfItemDetails>> GetAllBooksAsync()
        {
            return bookshelvesCollection.Query().ToListAsync();
        }

        public Task AddBookAsync(BookshelfItemDetails bookshelfItem)
        {
            return bookshelvesCollection.InsertAsync(bookshelfItem);
        }

        public async Task<IEnumerable<BookshelfItemDetails>> GetBooksByBookshelf(ReadingStatus status)
        {
            return bookshelvesCollection.FindAll().Where(c => c.ReadingStatus == status);
        }
    }
}
