using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Bookshelf.Mobile.ViewModels;
using Xamarin.Bookshelf.Mobile.Views;

namespace Xamarin.Bookshelf.Mobile
{
    public class ViewModelLocator
    {
        public BookshelvesPageViewModel BookshelvesPageViewModel => new BookshelvesPageViewModel();
        public BookSearchPageViewModel BookSearchPageViewModel => new BookSearchPageViewModel();
    }
}
