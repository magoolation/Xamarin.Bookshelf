using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Bookshelf.Shared.Models
{
    public class UserBookshelf
    {
        public string UserId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ReadingStatus ReadingStatus { get; set; }
        public int Count { get; set; }
        public BookshelfItemDetails[] Items { get; set; }
    }
}
