using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Bookshelf.Shared.Models
{
    public enum BookAction
    {
        AddToLibrary = 1,
        WriteReview,
        StartReading,
        FinishReading,
        AbandomReading,
        ShareReading,
        RecommendBook,
        UpdateReadingStatus
    }
}
