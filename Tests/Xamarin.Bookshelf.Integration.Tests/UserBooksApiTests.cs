using FluentAssertions;
using Refit;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Bookshelf.Shared;
using Xamarin.Bookshelf.Shared.Models;
using Xamarin.Bookshelf.Shared.Services;
using Xunit;

namespace Xamarin.Bookshelf.Integration.Tests
{
    public class UserBooksApiTests
    {
        private readonly IBookService bookService;

        public UserBooksApiTests()
        {
            var client = new HttpClient(new HostKeyMessageNandler())
            {
                BaseAddress = new Uri(ApiRoutes.API_BASE_URL)
            };

            bookService = RestService.For<IBookService>(client);
        }

        [Fact(DisplayName ="User adds a book to their bookshelves")]
        public async Task Add_Book_To_Library()
        {
            var bookId = "RhqJWjZ_6o4C";

            var request = new BookRegistration()
            {                
                BookId = bookId,
                ReadingPosition = 0.2d,
                ReadingStatus = ReadingStatus.Reading
            };

            var itemId = await bookService.RegisterBookAsync(request);

            var item = await bookService.GetBookshelfItemAsync(itemId);

            item.BookId.Should().Be(bookId);
        }

        [Fact(DisplayName ="User reviews a book")]
        public async Task Review_Book()
        {
            var bookId = "RhqJWjZ_6o4C";
            var title = "Integration Test Title";
            var review = new BookReviewRegistration()
            {
                BookId = bookId,
                Title = title,
                Review = "Integration Test Review",
                Rating = 0.75d
            };

            await bookService.ReviewBookAsync(review);

            var reviews = await bookService.GetBookReviewsAsync(bookId);

            reviews.Should().Contain(r => r.BookId == bookId);
            reviews.Should().Contain(r => r.Title == title);
        }
    }
}
