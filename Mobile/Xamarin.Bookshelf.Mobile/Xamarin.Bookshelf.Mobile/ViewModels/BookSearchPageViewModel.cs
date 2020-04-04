using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Bookshelf.Shared.Models;
using Xamarin.Bookshelf.Shared.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Xamarin.Bookshelf.Mobile.ViewModels
{
    public class BookSearchPageViewModel : BaseViewModel
    {
        private ObservableCollection<Book> books = new ObservableCollection<Book>(Enumerable.Empty<Book>());
        private readonly IBookService bookService;

        public ObservableCollection<Book> Books
        {
            get => books;
            set => SetProperty(ref books, value);
        }

        public ICommand SearchCommand { get; }
        public ICommand DetailsCommand { get; }

        public BookSearchPageViewModel(IBookService bookService)
        {
            this.bookService = bookService;
            SearchCommand = new AsyncCommand<string>(SearchBooksAsync);
            DetailsCommand = new AsyncCommand<string>(ViewDetailsAsync);
        }

        private async Task ViewDetailsAsync(string bookId)
        {
            await Shell.Current.GoToAsync($"Details?bookId={bookId}");
        }

        private async Task SearchBooksAsync(string text)
        {
            try
            {
                IsBusy = true;

                var result = await bookService.SearchBookByTitleAsync(text).ConfigureAwait(false);
                if (result == null)
                {
                    result = Enumerable.Empty<Book>();
                }

                Books = new ObservableCollection<Book>(result);
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.DisplayAlert("Error", ex.Message, "OK"));
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}