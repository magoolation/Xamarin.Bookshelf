using FluentAssertions;
using FluentAssertions.Common;
using System;
using System.ComponentModel;
using Xamarin.Bookshelf.Shared.Models;
using Xunit;

namespace Xamarin.Bookshelf.Unit.Tests.Books
{
    public class BookActionTests
    {
        [Fact(DisplayName = "User abandon the reading of a book")]
        public void Abandon_Reading()
        {
            var book = new BookshelfItem()
            {
                ReadingStatus = ReadingStatus.Reading,
                ReadingPosition = 0.50d
            };

        book.AbandonReading();

            book.ReadingStatus.Should().Be(ReadingStatus.Abandoned);
    }

        [Fact(DisplayName = "User finishes reading a book")]
        public void Finish_Reading()
        {
            var book = new BookshelfItem()
            {
                ReadingStatus = ReadingStatus.Reading,
                ReadingPosition = 0.20d
            };

            book.FinishReading();

            book.ReadingStatus.Should().Be(ReadingStatus.Read);
            book.ReadingPosition.Should().Be(1.0d);
        }

        [Fact(DisplayName ="User updates reading position")]
        public void Update_Reading_Position()
        {
            var newPosition = 0.5d;
            var book = new BookshelfItem()
            {
                ReadingStatus = ReadingStatus.Reading,
                ReadingPosition = 0.3d
            };

            book.UpdateReadingPosition(newPosition);

            book.ReadingPosition.Should().Be(newPosition);
        }

        [Fact(DisplayName ="User tries to update the reading position to an invalid value")]
        public void Update_Invalid_Position_Should_Return_Error()
        {
            var invalidPosition1 = -1d;
            var invalidPosition2 = 2d;
            var book = new BookshelfItem()
            {
                ReadingStatus = ReadingStatus.Reading
            };

            Assert.Throws<ArgumentException>(() => book.UpdateReadingPosition(invalidPosition1));
            Assert.Throws<ArgumentException>(() => book.UpdateReadingPosition(invalidPosition2));
        }

        [Fact(DisplayName ="User tries to abandon a book book not in reading status")]
        public void Abandon_Not_Reading_Book()
        {
            var book = new BookshelfItem()
            {
                ReadingStatus = ReadingStatus.Read
            };

            Assert.Throws<InvalidOperationException>(() => book.AbandonReading());
        }

        [Fact(DisplayName ="User tries to finish reading a book not in reading status")]
        public void Finish_Not_Reading_Book()
        {
            var book = new BookshelfItem()
            {
                ReadingStatus = ReadingStatus.Abandoned
            };

            Assert.Throws<InvalidOperationException>(() => book.FinishReading());
        }
    }
}
