using System.Collections.Generic;
using Xamarin.Bookshelf.Shared.Models;

namespace Xamarin.Bookshelf.Shared
{
    public static class EnumDescriptions
    {
        public static readonly Dictionary<ReadingStatus, string> ReadingStatuses = new Dictionary<ReadingStatus, string>()
        {
            { ReadingStatus.WantToRead, "I want to read" },
            { ReadingStatus.Reading, "I am reading" },
            { ReadingStatus.Read, "I already read" },
        };

        public static readonly Dictionary<BookAction, string> BookActions = new Dictionary<BookAction, string>()
        {
            { BookAction.AddToLibrary, "Add To Library" },
            { BookAction.WriteReview, "Write a Review" },
            { BookAction.StartReading, "Start Reading" },
            { BookAction.AbandomReading, "Abandom Reading" },{ BookAction.FinishReading, "Finish Reading" },
            { BookAction.ShareReading, "Share Reading Status" },
            { BookAction.RecommendBook, "Recommend this Book" },
            { BookAction.UpdateReadingStatus, "Update Reading Status" },
        };
}
}