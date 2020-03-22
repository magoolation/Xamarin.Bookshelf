using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.CosmosDB;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Xamarin.Bookshelf.Shared;
using Xamarin.Bookshelf.Shared.Models;
using ABookshelf = Xamarin.Bookshelf.Shared.Models.Bookshelf;

namespace Xamarin.Bookshelf.Functions
{
    public class UserBooksFunctions
    {
        [FunctionName("ReviewBook")]
        public IActionResult ReviewBook(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = ApiRoutes.API_SEND_BOOK_REVIEW)] HttpRequest req,
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
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = ApiRoutes.API_REGISTER_BOOK)] HttpRequest req,
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
            ILogger log)
        {
            if (bookshelves == null || !bookshelves.Any())
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(bookshelves);
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
    }
}