using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.CosmosDB;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Xamarin.Bookshelf.Functions.GoogleBooks;
using Xamarin.Bookshelf.Shared;
using Xamarin.Bookshelf.Shared.Models;
using ABookshelf = Xamarin.Bookshelf.Shared.Models.BookshelfItem;

namespace Xamarin.Bookshelf.Functions
{
    public class UserBooksFunctions
    {
        private const string API_KEY = "apiKey";
        private readonly GoogleBooksApiService googleBooksApi;
        private readonly string apiKey;

        public UserBooksFunctions(GoogleBooksApiService googleBooksApi, IConfiguration configuration)
        {
            this.googleBooksApi = googleBooksApi;
            this.apiKey = configuration[API_KEY];
        }

        [FunctionName("ReviewBook")]
        public IActionResult ReviewBook(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = ApiRoutes.API_REVIEWS)] HttpRequest req,
            [CosmosDB(
            databaseName: Constants.DATABASE_NAME,
            collectionName: Constants.REVIEWS_COLLECTION_NAME,
            ConnectionStringSetting = Constants.CONNECTION_STRING_SETTING,
            CreateIfNotExists = true)] out dynamic document,
            ILogger log)
        {
            var response = new StreamReader(req.Body).ReadToEnd();
            dynamic review = JsonConvert.DeserializeObject<BookReview>(response);

            document = review;

            return new OkResult();
        }

        [FunctionName("RegisterBook")]
        public IActionResult RegisterBook(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = ApiRoutes.API_BOOKSHELVES)] HttpRequest req,
            [CosmosDB(
            databaseName: Constants.DATABASE_NAME,
            collectionName: Constants.BOOKS_COLLECTION_NAME,
            ConnectionStringSetting = Constants.CONNECTION_STRING_SETTING,
            CreateIfNotExists = true)] out dynamic document,
            ILogger log)
        {
            var response = new StreamReader(req.Body).ReadToEnd();
            dynamic bookshelf = JsonConvert.DeserializeObject<ABookshelf>(response);

            document = bookshelf;

            return new OkResult();
        }

        [FunctionName("GetBookshelves")]
        public IActionResult GetBookshelves(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = ApiRoutes.API_GET_USER_BOOKS)] HttpRequest req,
            [CosmosDB(
            databaseName: Constants.DATABASE_NAME,
            collectionName: Constants.BOOKS_COLLECTION_NAME,
            ConnectionStringSetting = Constants.CONNECTION_STRING_SETTING,
            SqlQuery = "select * from Books b where b.UserId = {userId} order by b.ReadingStatus")] IEnumerable<ABookshelf> bookshelves,
            ILogger log,
            string userId)
        {
            string ipAddress = req.Headers["x-forwarded-for"];
            IEnumerable<UserBookshelf> userBookshelves = Enumerable.Empty < UserBookshelf>();

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

        private async Task<BookshelfItem[]> ConvertToBoksehlfItems(IEnumerable<ABookshelf> bookshelves, string ipAddress)
        {
            var books = new List<BookshelfItem>();
            foreach(var bookshelf in bookshelves)
            {
                var book = await googleBooksApi.Endpoint.GetBookById(bookshelf.BookId, apiKey, ipAddress);
                bookshelf.Book = ConvertToBook(book);
                books.Add(bookshelf);                   
            }
            return books.ToArray();
        }

        [FunctionName("GetReviews")]
        public IActionResult GetReviews(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = ApiRoutes.API_GET_BOOK_REVIEWS)] HttpRequest req,
            [CosmosDB(
            databaseName: Constants.DATABASE_NAME,
            collectionName: Constants.REVIEWS_COLLECTION_NAME,
            ConnectionStringSetting = Constants.CONNECTION_STRING_SETTING,
            SqlQuery = "select * from Reviews r where r.BookId = {bookId} order by r.UserId")] IEnumerable<ABookshelf> reviews,
            ILogger log)
        {
            if (reviews == null || !reviews.Any())
            {
                return new NotFoundResult();
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