using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Bookshelf.Functions.GoogleBooks;
using Xamarin.Bookshelf.Shared;
using Xamarin.Bookshelf.Shared.Models;

namespace Xamarin.Bookshelf.Functions
{
    public class GoogleBooksFunctions
    {
        private const string API_KEY = "apiKey";
        private readonly GoogleBooksApiService googleBooksApi;
        private readonly string apiKey;

        public GoogleBooksFunctions(GoogleBooksApiService googleBooksApi, IConfiguration configuration)
        {
            this.googleBooksApi = googleBooksApi;
            this.apiKey = configuration[API_KEY];
        }

        [FunctionName("SearchBook")]
        public async Task<IActionResult> SearchBooks(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = ApiRoutes.API_BOOKS)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            IEnumerable<Book> books = Enumerable.Empty<Book>();
            if (req.Query.TryGetValue("title", out var titles))
            {
                var volumes = await googleBooksApi.Endpoint.SearchBookByTitleAsync(titles.First(), apiKey);
                if (volumes != null && volumes.totalItems > 0)
                {
                    books = volumes.items.Select(ConvertToBook);
                }
            }
            else if (req.Query.TryGetValue("author", out var authors))
            {
                var volumes = await googleBooksApi.Endpoint.SearchBookByAuthorAsync(authors.First(), apiKey);
                if (volumes != null && volumes.totalItems > 0)
                {
                    books = volumes.items.Select(ConvertToBook); 
                }
            }
            else
            {
                return new BadRequestResult();
            }

            return new OkObjectResult(books);
        }

        private Book ConvertToBook(Volume volume)
        {
            return new Book()
            {
                BookId = volume.id,
                Title = volume.volumeInfo.title,
                SubTitle = volume.volumeInfo.subtitle,
                Summary = volume.volumeInfo.description,
                Authors = volume.volumeInfo.authors,
                Publisher = volume.volumeInfo.publisher,
                PublishedDate = volume.volumeInfo.publishedDate,
                Categories = volume.volumeInfo.categories,
                MainCategory = volume.volumeInfo.mainCategory,
                Language = volume.volumeInfo.language,
                PageCount = volume.volumeInfo.pageCount,
                Price = (decimal?)volume.saleInfo.listPrice?.amount,
                Rating = volume.volumeInfo.averageRating,
                RatingCount = volume.volumeInfo.ratingCount,
                ThumbnailUrl = volume.volumeInfo.imageLinks?.thumbnail,
                SmallThumbnailUrl = volume.volumeInfo.imageLinks?.smallThumbnail,
                SmallUrl   = volume.volumeInfo.imageLinks?.small,
                MediumUrl = volume.volumeInfo.imageLinks?.medium,
                LargeUrl = volume.volumeInfo.imageLinks?.large,
                ExtraLargeUrl = volume.volumeInfo.imageLinks?.extraLarge,
            };
        }

        [FunctionName("GetBook")]
        public async Task<IActionResult> GetBook(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = ApiRoutes.API_GET_BOOK_DETAILS)] HttpRequest req,
            string id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            Book book = default(Book);

            Volume volume = await googleBooksApi.Endpoint.GetBookById(id, apiKey);
            if (volume != null)
            {
                book = ConvertToBook(volume);
            }

            return new OkObjectResult(book);
        }
    }
}