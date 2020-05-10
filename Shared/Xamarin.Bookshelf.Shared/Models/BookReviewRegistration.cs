using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Bookshelf.Shared.Models
{
    public class BookReviewRegistration
    {
        public string BookId { get; set; }
        public string Title { get; set; }
        public double Rating { get; set; }
        public string Review { get; set; }
    }
}
