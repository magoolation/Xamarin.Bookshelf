using Refit;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Bookshelf.Shared;
using Xamarin.Bookshelf.Shared.Models;
using Xamarin.Bookshelf.Shared.Services;
using Xunit;

namespace Xamarin.Bookshelf.Integration.Tests
{
    public class BooksApiTests
    {
        [Fact]
        public async Task SearchBooksByTitle()
        {
            IBookService service = RestService.For<IBookService>(ApiRoutes.API_BASE_URL);

            var books = await service.SearchBookByTitle("mochileiro");

            Assert.Equal(10, books.Count());
        }
    }
}
