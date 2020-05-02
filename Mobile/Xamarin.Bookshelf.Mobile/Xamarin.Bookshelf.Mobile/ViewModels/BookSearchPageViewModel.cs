using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Bookshelf.Mobile.Services;
using Xamarin.Bookshelf.Shared.Models;
using Xamarin.Bookshelf.Shared.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Xamarin.Bookshelf.Mobile.ViewModels
{
    public class BookSearchPageViewModel : BaseViewModel
    {
        private readonly IBookService bookService;
        private readonly IBookRepository repository;

        private ObservableCollection<Book> books = new ObservableCollection<Book>(Enumerable.Empty<Book>());
        public ObservableCollection<Book> Books
        {
            get => books;
            set => SetProperty(ref books, value);
        }

        private IEnumerable<BookshelfItem> wantToRead;
        public IEnumerable<BookshelfItem>  WantToRead
        {
            get => wantToRead;
            set => SetProperty(ref wantToRead, value);
        }

        public ICommand SearchCommand { get; }
        public ICommand DetailsCommand { get; }

        public BookSearchPageViewModel(BookService bookService, IBookRepository repository)
        {
            this.bookService = bookService.Endpoint;
            this.repository = repository;

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

                var byTitle = bookService.SearchBookByTitleAsync(text);
                var byAuthor = bookService.SearchBookByAuthorAsync(text);

                await Task.WhenAll(byTitle, byAuthor).ConfigureAwait(false);

                var result = (byTitle.Result ?? Enumerable.Empty<Book>())
                    .Concat(byAuthor.Result ?? Enumerable.Empty<Book>());
                
                Books = new ObservableCollection<Book>(result);
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                var items = Enumerable.Empty<BookshelfItem>();
                var cached = await repository.GetBooksByBookshelf(ReadingStatus.WantToRead);
                if (cached != null && cached.Any())
                {
                    items = cached;
                }

                WantToRead = items;
            }
            catch(Exception ex)
                {
                await DisplayAlertAsync("Error", ex.Message, "OK");
            }
            }
    }
}