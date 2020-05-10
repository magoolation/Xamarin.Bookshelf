using FluentAssertions;
using Refit;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Bookshelf.Shared;
using Xamarin.Bookshelf.Shared.Services;
using Xunit;

namespace Xamarin.Bookshelf.Integration.Tests
{
    public class BooksApiTests
    {
        private IBookService bookService;

        public BooksApiTests()
        {
            var client = new HttpClient(new HostKeyMessageNandler())
            {
                BaseAddress = new Uri(ApiRoutes.API_BASE_URL)
            };

            bookService = RestService.For<IBookService>(client);
        }

        [Fact(DisplayName ="User search books by title")]        
        public async Task Search_Books_by_Title()
        {
            var title = "mochileiro";
            var books = await bookService.SearchBookByTitleAsync(title);

            books.Count().Should().Be(10);
            books.First().Title.Should().Contain(title);
        }

        [Fact(DisplayName ="User search books by Author")]
        public async Task Search_Books_by_Author()
        {
            var author = "Douglas Adams";
            var books = await bookService.SearchBookByAuthorAsync(author);

            books.Count().Should().Be(10);
            books.First().Authors.Should().ContainSingle(author);
        }

        [Fact(DisplayName ="User gets a single book details")]
        public async Task Get_Book_Details()
        {
            var bookId = "RhqJWjZ_6o4C";
            var book = await bookService.GetBookDetailsAsync(bookId);

            book.BookId.Should().Be(bookId);
        }
    }
}