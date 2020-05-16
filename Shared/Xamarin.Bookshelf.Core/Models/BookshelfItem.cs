using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using Xamarin.Bookshelf.Shared.Models;

namespace Xamarin.Bookshelf.Core.Models
{
    public class BookshelfItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string BookId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ReadingStatus ReadingStatus { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public void AbandonReading()
        {
            if (ReadingStatus!= ReadingStatus.Reading)
                    {
                throw new InvalidOperationException();
            }
            ReadingStatus = ReadingStatus.Abandoned;            
        }

        public double ReadingPosition { get; set; }

        public void FinishReading()
        {
            if (ReadingStatus != ReadingStatus.Reading)
            {
                throw new InvalidOperationException();
            }
            ReadingStatus = ReadingStatus.Read;
            ReadingPosition = 1.0d;
        }

        public void UpdateReadingPosition(double newPosition)
        {
            if (newPosition < 0d || newPosition > 1d)
                    {
                throw new ArgumentException(nameof(newPosition));
            }
            ReadingPosition = newPosition;
        }
    }
}
