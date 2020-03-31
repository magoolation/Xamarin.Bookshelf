using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Bookshelf.Shared.Models;

namespace Xamarin.Bookshelf.Shared
{
    public static class EnumDescriptions
    {
        public static readonly Dictionary<ReadingStatus, string> ReadingStatuses = new Dictionary<ReadingStatus, string>()
        {
            { ReadingStatus.Reading, "I am reading" },
            { ReadingStatus.Read, "I already read" },
            { ReadingStatus.WantToRead, "I want to read" },
        };
}
}