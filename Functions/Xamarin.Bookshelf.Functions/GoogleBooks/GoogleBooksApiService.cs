using Refit;
using System;
using System.Net.Http;

namespace Xamarin.Bookshelf.Functions.GoogleBooks
{
    public class GoogleBooksApiService
    {
        private const string BASE_URL = "https://www.googleapis.com";

        public IGoogleBooksApi Endpoint { get; }

        public GoogleBooksApiService(HttpClient client)
        {
            client.BaseAddress = new Uri(BASE_URL);
            Endpoint = RestService.For<IGoogleBooksApi>(client);
        }
    }
}
