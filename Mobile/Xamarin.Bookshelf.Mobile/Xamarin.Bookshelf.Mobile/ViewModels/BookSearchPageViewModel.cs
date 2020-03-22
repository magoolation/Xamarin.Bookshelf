using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Bookshelf.Shared.Models;
using Xamarin.Bookshelf.Shared.Services;
using Xamarin.Forms;

namespace Xamarin.Bookshelf.Mobile.ViewModels
{
    public class BookSearchPageViewModel: BaseViewModel
    {
        private string searchText;
        public string SearchText
        {
            get => searchText;
            set => SetProperty(ref searchText, value);
        }

        private ObservableCollection<Book> books = new ObservableCollection<Book>(Enumerable.Empty<Book>());
        private readonly IBookService bookService;

        public ObservableCollection<Book> Books
        {
            get => books;
            set => SetProperty(ref books, value);
        }

        public ICommand SearchCommand { get; }

        public BookSearchPageViewModel(IBookService bookService)
        {
            this.bookService = bookService;
            SearchCommand = new Command(SearchBooks, CanSearchBooks);
        }

        private async void SearchBooks(object arg)
        {
            await SearchBooksAsync();
        }

        private async Task SearchBooksAsync()
        {
            try
            {
                IsBusy = true;

                var result = await bookService.SearchBookByTitleAsync(SearchText).ConfigureAwait(false);
                if (result == null)
                {
                    result = Enumerable.Empty<Book>();
                }

                Books = new ObservableCollection<Book>(result);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanSearchBooks(object arg)
        {
            return !string.IsNullOrWhiteSpace(searchText);
        }
    }
}
