using Refit;
using System.Threading.Tasks;

namespace Xamarin.Bookshelf.Functions.GoogleBooks
{
    public interface IGoogleBooksApi
    {
        [Get("/books/v1/volumes?q=intitle:{title}&key={apiKey}&userIp={ipAddress}")]
        Task<GoogleBooksSearchResult> SearchBookByTitleAsync(string title, string apiKey, string ipAddress);
        [Get("/books/v1/volumes?q=inauthor:{author}&key={apiKey}&userIp={ipAddress}")]
        Task<GoogleBooksSearchResult> SearchBookByAuthorAsync(string author, string apiKey, string ipAddress);
        [Get("/books/v1/volumes/{bookId}?key={apiKey}&userIp={ipAddress}")]
        Task<Volume> GetBookById(string bookId, string apiKey, string ipAddress);
    }
}
