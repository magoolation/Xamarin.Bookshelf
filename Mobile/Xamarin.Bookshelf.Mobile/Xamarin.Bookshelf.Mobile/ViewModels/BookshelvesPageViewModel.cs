using Xamarin.Bookshelf.Shared.Services;

namespace Xamarin.Bookshelf.Mobile.ViewModels
{
    public class BookshelvesPageViewModel : BaseViewModel
    {
        private readonly IBookService bookService;

        public BookshelvesPageViewModel(IBookService bookService)
        {
            this.bookService = bookService;
        }
    }
}
