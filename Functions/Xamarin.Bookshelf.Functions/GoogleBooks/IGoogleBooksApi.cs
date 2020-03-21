using Refit;
using System.Threading.Tasks;

namespace Xamarin.Bookshelf.Functions.GoogleBooks
{
    public interface IGoogleBooksApi
    {
        [Get("/books/v1/volumes?q=intitle:{title}&key={apiKey}")]
        Task<GoogleBooksSearchResult> SearchBookByTitleAsync(string title, string apiKey);
        [Get("/books/v1/volumes?q=inauthor:{author}&key={apiKey}")]
        Task<GoogleBooksSearchResult> SearchBookByAuthorAsync(string author, string apiKey);
        [Get("/books/v1/volumes/{bookId}?key={apiKey}")]
        Task<Volume> GetBookById(string bookId, string apiKey);
    }
}
