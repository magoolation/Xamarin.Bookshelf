using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Xamarin.Bookshelf.Shared.Models
{
    public enum ReadingStatus
    {
        WantToRead = 1,
        Reading = 2,
        Read = 3
    }

    public class BookshelfItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string BookId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ReadingStatus ReadingStatus { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public double ReadingPosition { get; set; }
        [JsonIgnore]
        public Book Book {get; set; }
    }
}
