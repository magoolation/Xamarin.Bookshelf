using System;

namespace Xamarin.Bookshelf.Shared.Models
{
    public class BookshelfItemDetails
    {
        public string BookId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string[] Authors { get; set; }
        public string Summary { get; set; }
        public double Rating { get; set; }
        public decimal? Price { get; set; }
        public string Publisher { get; set; }
        public int PageCount { get; set; }
        public int RatingCount { get; set; }
        public string PublishedDate { get; set; }
        public string[] Categories { get; set; }
        public string Language { get; set; }
        public string MainCategory { get; set; }
        public string ThumbnailUrl { get; set; }
        public string SmallThumbnailUrl { get; set; }
        public string SmallUrl { get; set; }
        public string MediumUrl { get; set; }
        public string LargeUrl { get; set; }
        public string ExtraLargeUrl { get; set; }
        public ReadingStatus ReadingStatus { get; set; }
        public double ReadingPosition { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}