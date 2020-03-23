using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Bookshelf.Shared.Services;

namespace Xamarin.Bookshelf.Mobile.ViewModels
{
    public class ReviewBookPageViewModel: BaseViewModel
    {
        private readonly IBookService bookService;

        public ReviewBookPageViewModel(IBookService bookService)
        {
            this.bookService = bookService;
        }
    }
}
