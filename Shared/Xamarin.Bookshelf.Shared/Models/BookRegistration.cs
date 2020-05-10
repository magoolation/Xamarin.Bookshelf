using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Bookshelf.Shared.Models
{
    public class BookRegistration
    {
        public string Id { get; set; }
        public double ReadingPosition { get; set; }
        public string BookId { get; set; }
        public ReadingStatus ReadingStatus { get; set; }
    }
}
