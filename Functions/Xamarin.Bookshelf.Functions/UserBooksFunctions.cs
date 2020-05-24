using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xamarin.Bookshelf.Core.Models;
using Xamarin.Bookshelf.Functions.GoogleBooks;
using Xamarin.Bookshelf.Shared;
using Xamarin.Bookshelf.Shared.Models;

namespace Xamarin.Bookshelf.Functions
{
    public class UserBooksFunctions
    {
        private const string ADMIN_USER = "Integration Tests";
        private const string API_KEY = "apiKey";
        private readonly IGoogleBooksApi _googleBooksApi;
        private readonly string _apiKey;

        public UserBooksFunctions(IGoogleBooksApi googleBooksApi, IConfiguration configuration)
        {
            _googleBooksApi = googleBooksApi;
            _apiKey = configuration[API_KEY];
        }

        [FunctionName("ReviewBook")]
        public async Task<IActionResult> ReviewBookAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = ApiRoutes.API_REVIEWS)] HttpRequest req,
            [CosmosDB(
            databaseName: Constants.DATABASE_NAME,
            collectionName: Constants.REVIEWS_COLLECTION_NAME,
            ConnectionStringSetting = Constants.CONNECTION_STRING_SETTING)] DocumentClient client,
            ILogger log,
            ClaimsPrincipal user)
        {
            log.LogInformation("Started ReviewBook function.");

            if (!user.Identity.IsAuthenticated)
            {
                log.LogError("User is not authenticated");

                return new UnauthorizedResult();
            }

            log.LogInformation("Reading message body");

            var response = new StreamReader(req.Body).ReadToEnd();
            BookReviewRegistration review = JsonConvert.DeserializeObject<BookReviewRegistration>(response);

            var id = Guid.NewGuid().ToString();

            log.LogInformation($"Review id: {id}");

            var document = new BookReview()
            {
                Id = id,
                UserId = user.GetUserId() ?? ADMIN_USER,
                BookId = review.BookId,
                Title = review.Title,
                Rating = review.Rating,
                Review = review.Review,
                CreatedAt = DateTimeOffset.UtcNow
            };

            var options = new RequestOptions()
            {
                PartitionKey = new PartitionKey(document.BookId)
            };

            var uri = UriFactory.CreateDocumentCollectionUri(Constants.DATABASE_NAME, Constants.REVIEWS_COLLECTION_NAME);

            _ = await client.CreateDocumentAsync(uri, document, options, true);

            log.LogInformation("ReviewBook function finished successfully.");

            return new OkObjectResult(id);
        }

        [FunctionName("RegisterBook")]
        public async Task<IActionResult> RegisterBookAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = ApiRoutes.API_USER_BOOKSHELVES)] HttpRequest req,
            [CosmosDB(
            databaseName: Constants.DATABASE_NAME,
            collectionName: Constants.BOOKS_COLLECTION_NAME,
            ConnectionStringSetting = Constants.CONNECTION_STRING_SETTING)] DocumentClient client,
            ILogger log,
            ClaimsPrincipal user)
        {
            log.LogInformation("Started REgisterBook function.");

            if (!user.Identity.IsAuthenticated)
            {
                log.LogError("User is not authenticated.");
                return new UnauthorizedResult();
            }

            log.LogInformation("Reading message body.");

            var response = new StreamReader(req.Body).ReadToEnd();
            BookRegistration bookshelf = JsonConvert.DeserializeObject<BookRegistration>(response);

            var id = Guid.NewGuid().ToString();

            log.LogInformation($"Bookshelf Item id: {id}");

            var document = new BookshelfItem()
            {
                Id = id,
                UserId = user.GetUserId() ?? ADMIN_USER,
                BookId = bookshelf.BookId,
                CreatedAt = DateTime.UtcNow,
                ReadingPosition = bookshelf.ReadingPosition,
                ReadingStatus = bookshelf.ReadingStatus
            };

            var uri = UriFactory.CreateDocumentCollectionUri(Constants.DATABASE_NAME, Constants.BOOKS_COLLECTION_NAME);
            var options = new RequestOptions()
            {
                PartitionKey = new PartitionKey(user.GetUserId() ?? ADMIN_USER)
            };

            _ = await client.CreateDocumentAsync(uri, document, options, true);

            log.LogInformation("RegiterBook function finished successfully");

            return new OkObjectResult(id);
        }

        [FunctionName("GetBookshelfItem")]
        public async Task<IActionResult> GetBokshelfItemAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = ApiRoutes.API_GET_USER_BOOK)] HttpRequest req,
            [CosmosDB(Constants.DATABASE_NAME, Constants.BOOKS_COLLECTION_NAME,
            ConnectionStringSetting = Constants.CONNECTION_STRING_SETTING)] DocumentClient client,
            ILogger log,
            string id,
            ClaimsPrincipal user)
        {
            log.LogInformation("Started function GetBookshelfItem.");

            if (!user.Identity.IsAuthenticated)
            {
                log.LogError("User i is not authenticated");
                return new UnauthorizedResult();
            }

            var options = new FeedOptions()
            {
                PartitionKey = new PartitionKey(user.GetUserId() ?? ADMIN_USER)
            };

            var uri = UriFactory.CreateDocumentCollectionUri(Constants.DATABASE_NAME, Constants.BOOKS_COLLECTION_NAME);
            BookshelfItem item = client.CreateDocumentQuery<BookshelfItem>(uri, options)
                .Where(d => d.Id == id)
                .AsEnumerable().FirstOrDefault();

            string ipAddress = req.Headers["x-forwarded-for"];
            log.LogInformation("Creating the response.");

            var bookshelf = new BookshelfItemDetails()
            {
                BookId = item.BookId,
                ReadingStatus = item.ReadingStatus,
                ReadingPosition = item.ReadingPosition,
            };

            log.LogInformation("Populating the book data");

            await PopulateBookshelfItemDetails(ipAddress, bookshelf, log);

            log.LogInformation("GetBookshelfItem finished successfully");

            return new OkObjectResult(bookshelf);
        }

        [FunctionName("GetBookshelves")]
        public async Task<IActionResult> GetBookshelves(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = ApiRoutes.API_USER_BOOKSHELVES)] HttpRequest req,
            [CosmosDB(
            databaseName: Constants.DATABASE_NAME,
            collectionName: Constants.BOOKS_COLLECTION_NAME,
            ConnectionStringSetting = Constants.CONNECTION_STRING_SETTING)] IDocumentClient document,
            ClaimsPrincipal user,
            ILogger log)
        {
            if (!user.Identity.IsAuthenticated)
            {
                return new UnauthorizedResult();
            }

            string ipAddress = req.Headers["x-forwarded-for"];
            IEnumerable<UserBookshelf> userBookshelves = Enumerable.Empty<UserBookshelf>();
            List<BookshelfItem> bookshelves = new List<BookshelfItem>();

            var uri = UriFactory.CreateDocumentCollectionUri(Constants.DATABASE_NAME, Constants.BOOKS_COLLECTION_NAME);
            var options = new FeedOptions()
            {
                PartitionKey = new PartitionKey(user.GetUserId() ?? ADMIN_USER)
            };

            var query = document.CreateDocumentQuery<BookshelfItem>(uri, options)
                .OrderByDescending(b => b.CreatedAt)
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
                catch (Exception ex)
                {
                    log.LogError(ex.Message);
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
                    UserId = user.GetUserId() ?? ADMIN_USER,
                    Count = books.Count(),
                    Items = books.Select(b => new BookshelfItemDetails()
                    {
                        BookId = b.BookId,
                        ReadingStatus = status,
                        ReadingPosition = b.ReadingPosition,
                        CreatedAt = b.CreatedAt
                    }).ToArray()
                });
            }

            await GetBookshelfItemsDetails(userBookshelves.SelectMany(b => b.Items), ipAddress, log);

            return new OkObjectResult(userBookshelves);
        }

        private async Task GetBookshelfItemsDetails(IEnumerable<BookshelfItemDetails> bookshelves, string ipAddress, ILogger log)
        {
            foreach (var bookshelf in bookshelves)
            {
                await PopulateBookshelfItemDetails(ipAddress, bookshelf, log);
            }
        }

        private async Task PopulateBookshelfItemDetails(string ipAddress, BookshelfItemDetails bookshelf, ILogger log)
        {
            try
            {
                var book = await _googleBooksApi.GetBookById(bookshelf.BookId, _apiKey, ipAddress);
                bookshelf.Title = book.volumeInfo.title;
                bookshelf.SubTitle = book.volumeInfo.subtitle;
                bookshelf.Summary = book.volumeInfo.description;
                bookshelf.Authors = book.volumeInfo.authors;
                bookshelf.PageCount = book.volumeInfo.pageCount;
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
            }
        }

        [FunctionName("GetBookReviews")]
        public async Task<IActionResult> GetReviewsAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = ApiRoutes.API_GET_BOOK_REVIEWS)] HttpRequest _,
            [CosmosDB(
            databaseName: Constants.DATABASE_NAME,
            collectionName: Constants.REVIEWS_COLLECTION_NAME,
            ConnectionStringSetting = Constants.CONNECTION_STRING_SETTING)]DocumentClient client,
            string bookId,
            ILogger log,
            ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
            {
                return new UnauthorizedResult();
            }

            List<UserBookReview> reviews = new List<UserBookReview>();

            var uri = UriFactory.CreateDocumentCollectionUri(Constants.DATABASE_NAME, Constants.REVIEWS_COLLECTION_NAME);
            var options = new FeedOptions()
            {
                PartitionKey = new PartitionKey(bookId)
            };

            var query = client.CreateDocumentQuery<BookReview>(uri, options)
                .OrderByDescending(r => r.CreatedAt)
                .AsDocumentQuery();

            while (query.HasMoreResults)
            {
                try
                {
                    foreach (BookReview result in await query.ExecuteNextAsync<BookReview>())
                    {
                        var review = new UserBookReview()
                        {
                            BookId = result.BookId,
                            UserId = result.UserId,
                            Title = result.Title,
                            Review = result.Review,
                            Rating = result.Rating,
                            CreatedAt = result.CreatedAt
                        };
                        reviews.Add(review);
                    }
                }
                catch (Exception ex)
                {
                    log.LogError(ex.Message);
                    break;
                }
            }

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