using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Bookshelf.Shared.Models
{
    public class UserBookReview
    {
        public string BookId { get; set; }
        public string Title { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public dynamic Rating { get; set; }
        public dynamic Review { get; set; }
        public dynamic UserId { get; set; }
    }
}
