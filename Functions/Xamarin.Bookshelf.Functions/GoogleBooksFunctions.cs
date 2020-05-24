using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xamarin.Bookshelf.Functions.GoogleBooks;
using Xamarin.Bookshelf.Shared;
using Xamarin.Bookshelf.Shared.Models;

namespace Xamarin.Bookshelf.Functions
{
    public class GoogleBooksFunctions
    {
        private const string API_KEY = "apiKey";
        private readonly IGoogleBooksApi _googleBooksApi;
        private readonly string _apiKey;

        public GoogleBooksFunctions(IGoogleBooksApi googleBooksApi, IConfiguration configuration)
        {
            _googleBooksApi = googleBooksApi;
            _apiKey = configuration[API_KEY];
        }

        [FunctionName("SearchBook")]
        public async Task<IActionResult> SearchBooks(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ApiRoutes.API_BOOKS)] HttpRequest req,
            ILogger log,
            ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
            {
                return new UnauthorizedResult();
            }

            log.LogInformation("C# HTTP trigger function processed a request.");

            string ipAddress = req.Headers["x-forwarded-for"];

            IEnumerable<BookSummary> books = Enumerable.Empty<BookSummary>();
            if (req.Query.TryGetValue("title", out var titles))
            {
                var volumes = await _googleBooksApi.SearchBookByTitleAsync(titles.First(), _apiKey, ipAddress);
                if (volumes != null && volumes.totalItems > 0)
                {
                    books = volumes.items.Select(b => ConvertToBookSummary(b));
                }
            }
            else if (req.Query.TryGetValue("author", out var authors))
            {
                var volumes = await _googleBooksApi.SearchBookByAuthorAsync(authors.First(), _apiKey, ipAddress);
                if (volumes != null && volumes.totalItems > 0)
                {
                    books = volumes.items.Select(b => ConvertToBookSummary(b));
                }
            }
            else
            {
                return new BadRequestResult();
            }

            return new OkObjectResult(books);
        }

        private BookSummary ConvertToBookSummary(Volume volume)
        {
            return new BookSummary()
            {
                BookId = volume.id,
                Title = volume.volumeInfo.title,
                SubTitle = volume.volumeInfo.subtitle,
                Authors = volume.volumeInfo.authors,
                Publisher = volume.volumeInfo.publisher,
                PublishedDate = volume.volumeInfo.publishedDate,
                PageCount = volume.volumeInfo.pageCount,
                ThumbnailUrl = volume.volumeInfo.imageLinks?.thumbnail,
                SmallThumbnailUrl = volume.volumeInfo.imageLinks?.smallThumbnail,
                SmallUrl = volume.volumeInfo.imageLinks?.small,
                MediumUrl = volume.volumeInfo.imageLinks?.medium,
                LargeUrl = volume.volumeInfo.imageLinks?.large,
                ExtraLargeUrl = volume.volumeInfo.imageLinks?.extraLarge,
            };

        }

        private BookDetails ConvertToBookDetails(Volume volume)
        {
            return new BookDetails()
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
                Rating = volume.volumeInfo.averageRating,
                RatingCount = volume.volumeInfo.ratingsCount,
                ThumbnailUrl = volume.volumeInfo.imageLinks?.thumbnail,
                SmallThumbnailUrl = volume.volumeInfo.imageLinks?.smallThumbnail,
                SmallUrl = volume.volumeInfo.imageLinks?.small,
                MediumUrl = volume.volumeInfo.imageLinks?.medium,
                LargeUrl = volume.volumeInfo.imageLinks?.large,
                ExtraLargeUrl = volume.volumeInfo.imageLinks?.extraLarge,
            };
        }

        [FunctionName("GetBook")]
        public async Task<IActionResult> GetBook(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ApiRoutes.API_GET_BOOK_DETAILS)] HttpRequest req,
            string id,
            ILogger log,
            ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
            {
                return new UnauthorizedResult();
            }

            log.LogInformation("C# HTTP trigger function processed a request.");
            string ipAddress = req.Headers["x-forwarded-for"];

            BookDetails book = default(BookDetails);

            Volume volume = await _googleBooksApi.GetBookById(id, _apiKey, ipAddress);
            if (volume != null)
            {
                book = ConvertToBookDetails(volume);
            }

            return new OkObjectResult(book);
        }
    }
}