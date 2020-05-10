using Newtonsoft.Json;
using System;

namespace Xamarin.Bookshelf.Core.Models
{
    public class BookReview
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string BookId { get; set; }
        public string UserId { get; set; }
        public double Rating { get; set; }
        public string Title { get; set; }
        public string Review { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
