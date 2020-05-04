using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Bookshelf.Functions.GoogleBooks;
using Xamarin.Bookshelf.Shared;
using Xamarin.Bookshelf.Shared.Models;
using System.Security.Claims;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace Xamarin.Bookshelf.Functions
{
    public class UserBooksFunctions
    {
        private const string API_KEY = "apiKey";
        private readonly IGoogleBooksApi googleBooksApi;
        private readonly string apiKey;

        public UserBooksFunctions(IGoogleBooksApi googleBooksApi, IConfiguration configuration)
        {
            this.googleBooksApi = googleBooksApi;
            this.apiKey = configuration[API_KEY];
        }

        [FunctionName("ReviewBook")]
        public IActionResult ReviewBook(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = ApiRoutes.API_REVIEWS)] HttpRequest req,
            [CosmosDB(
            databaseName: Constants.DATABASE_NAME,
            collectionName: Constants.REVIEWS_COLLECTION_NAME,
            ConnectionStringSetting = Constants.CONNECTION_STRING_SETTING)] out dynamic document,
            ILogger log,
            ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
            {
                document = null;
                return new UnauthorizedResult();
            }

            var response = new StreamReader(req.Body).ReadToEnd();
            dynamic review = JsonConvert.DeserializeObject<BookReview>(response);

            document = review;

            return new OkResult();
        }

        [FunctionName("RegisterBook")]
        public IActionResult RegisterBook(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = ApiRoutes.API_BOOKSHELVES)] HttpRequest req,
            [CosmosDB(
            databaseName: Constants.DATABASE_NAME,
            collectionName: Constants.BOOKS_COLLECTION_NAME,
            ConnectionStringSetting = Constants.CONNECTION_STRING_SETTING)] out dynamic document,
            ILogger log,
            ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
            {
                document = null;
                return new UnauthorizedResult();
            }

            var response = new StreamReader(req.Body).ReadToEnd();
            dynamic bookshelf = JsonConvert.DeserializeObject<BookshelfItem>(response);

            document = bookshelf;

            return new OkResult();
        }

        [FunctionName("GetBookshelves")]
        public async Task<IActionResult> GetBookshelves(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = ApiRoutes.API_GET_USER_BOOKS)] HttpRequest req,
            [CosmosDB(
            databaseName: Constants.DATABASE_NAME,
            collectionName: Constants.BOOKS_COLLECTION_NAME,
            ConnectionStringSetting = Constants.CONNECTION_STRING_SETTING)] IDocumentClient document,
            ILogger log,
            string userId,
            ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
            {
                return new UnauthorizedResult();
            }

            string ipAddress = req.Headers["x-forwarded-for"];
            IEnumerable<UserBookshelf> userBookshelves = Enumerable.Empty < UserBookshelf>();
            List<BookshelfItem> bookshelves = new List<BookshelfItem>();

            var uri = UriFactory.CreateDocumentCollectionUri(Constants.DATABASE_NAME, Constants.BOOKS_COLLECTION_NAME);

            var query = document.CreateDocumentQuery<BookshelfItem>(uri)
                .Where(d => d.UserId == userId)
                .AsDocumentQuery();

            while (query.HasMoreResults)
            {
                try
                {
                    foreach (BookshelfItem result in await query.ExecuteNextAsync<BookshelfItem>())
                    {
                        bookshelves.Add(result);
                    }
                }
                catch (System.Exception ex)
                {
                    //throw;
                    break;
                }
            }

            if (bookshelves != null && bookshelves.Any())
            {
                userBookshelves = bookshelves.GroupBy(
                    b => b.ReadingStatus,
                b => b,
                (status, books) => new UserBookshelf()
                {
                    ReadingStatus = status,
                    UserId = userId,
                    Count = books.Count(),
                    Items = ConvertToBoksehlfItems(books, ipAddress).GetAwaiter().GetResult()
                });
            }


            return new OkObjectResult(userBookshelves);
        }

        private async Task<BookshelfItem[]> ConvertToBoksehlfItems(IEnumerable<BookshelfItem> bookshelves, string ipAddress)
        {
            var books = new List<BookshelfItem>();
            foreach(var bookshelf in bookshelves)
            {
                try
                {
                    var book = await googleBooksApi.GetBookById(bookshelf.BookId, apiKey, ipAddress);
                    bookshelf.Book = ConvertToBook(book);
                    books.Add(bookshelf);
                }
                catch (System.Exception ex)
                {
                    // Add Log
                }
            }
            return books.ToArray();
        }

        [FunctionName("GetReviews")]
        public IActionResult GetReviews(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = ApiRoutes.API_GET_BOOK_REVIEWS)] HttpRequest req,
            [CosmosDB(
            databaseName: Constants.DATABASE_NAME,
            collectionName: Constants.REVIEWS_COLLECTION_NAME,
            ConnectionStringSetting = Constants.CONNECTION_STRING_SETTING,
            SqlQuery = "select * from Reviews r where r.BookId = {bookId} order by r.UserId")] IEnumerable<BookshelfItem> reviews,
            ILogger log)
        {            
            return new OkObjectResult(reviews);
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
                RatingCount = volume.volumeInfo.ratingsCount,
                ThumbnailUrl = volume.volumeInfo.imageLinks?.thumbnail,
                SmallThumbnailUrl = volume.volumeInfo.imageLinks?.smallThumbnail,
                SmallUrl = volume.volumeInfo.imageLinks?.small,
                MediumUrl = volume.volumeInfo.imageLinks?.medium,
                LargeUrl = volume.volumeInfo.imageLinks?.large,
                ExtraLargeUrl = volume.volumeInfo.imageLinks?.extraLarge,
            };
        }
    }
}