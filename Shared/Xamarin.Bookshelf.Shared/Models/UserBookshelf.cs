using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Bookshelf.Shared.Models
{
    public class UserBookshelf
    {
        public string UserId { get; set; }
        public ReadingStatus ReadingStatus { get; set; }
        public int Count { get; set; }
        public BookshelfItem[] Items { get; set; }
    }
}
