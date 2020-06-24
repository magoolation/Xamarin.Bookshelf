using AsyncAwaitBestPractices.MVVM;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Bookshelf.Mobile.Services;
using Xamarin.Bookshelf.Shared.Models;
using Xamarin.Bookshelf.Shared.Services;

namespace Xamarin.Bookshelf.Mobile.ViewModels
{
    public class BookSearchPageViewModel : BaseViewModel
    {
        private readonly IBookService bookService;
        private readonly IBookRepository repository;
        private readonly IDialogService _dialogService;
        private readonly IErrorHandlingService _errorHandlingService;
        private ObservableCollection<BookSummary> books = new ObservableCollection<BookSummary>(Enumerable.Empty<BookSummary>());
        public ObservableCollection<BookSummary> Books
        {
            get => books;
            set => Set(ref books, value);
        }

        private IEnumerable<BookshelfItemDetails> wantToRead;
        public IEnumerable<BookshelfItemDetails> WantToRead
        {
            get => wantToRead;
            set => Set(ref wantToRead, value);
        }

        public ICommand SearchCommand { get; }
        public ICommand DetailsCommand { get; }

        public BookSearchPageViewModel(IBookService bookService, IBookRepository repository, IDialogService dialogService, IErrorHandlingService errorHandlingService)
        {
            this.bookService = bookService;
            this.repository = repository;
            _dialogService = dialogService;
            _errorHandlingService = errorHandlingService;
            SearchCommand = new AsyncCommand<string>(SearchBooksAsync);
            DetailsCommand = new AsyncCommand<string>(ViewDetailsAsync);
        }

        private async Task ViewDetailsAsync(string bookId)
        {
            await Navigation.NavigateToAsync($"Details?bookId={bookId}");
        }

        private async Task SearchBooksAsync(string text)
        {
            try
            {
                IsBusy = true;

                var byTitle = bookService.SearchBookByTitleAsync(text);
                var byAuthor = bookService.SearchBookByAuthorAsync(text);

                await Task.WhenAll(byTitle, byAuthor).ConfigureAwait(false);

                var result = (byTitle.Result ?? Enumerable.Empty<BookSummary>())
                    .Concat(byAuthor.Result ?? Enumerable.Empty<BookSummary>());

                Books = new ObservableCollection<BookSummary>(result);
            }
            catch (ApiException apiError)
            {
                await _errorHandlingService.HandleApiExceptionAsync(apiError);
            }
            catch (Exception ex)
            {
                await _dialogService.DisplayAlertAsync("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            try
            {
                var items = Enumerable.Empty<BookshelfItemDetails>();
                var cached = await repository.GetBooksByBookshelf(ReadingStatus.WantToRead);
                if (cached != null && cached.Any())
                {
                    items = cached;
                }

                WantToRead = items;
            }
            catch (Exception ex)
            {
                await _dialogService.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        }
    }
}