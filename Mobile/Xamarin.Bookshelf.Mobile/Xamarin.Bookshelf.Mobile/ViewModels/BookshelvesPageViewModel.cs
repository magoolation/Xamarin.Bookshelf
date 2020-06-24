using AsyncAwaitBestPractices.MVVM;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Bookshelf.Mobile.Services;
using Xamarin.Bookshelf.Shared;
using Xamarin.Bookshelf.Shared.Models;
using Xamarin.Bookshelf.Shared.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Xamarin.Bookshelf.Mobile.ViewModels
{
    public class BookshelvesPageViewModel : BaseViewModel
    {
        private readonly IBookService bookService;
        private readonly IBookRepository repository;
        private readonly IAuthenticationTokenManager authenticationTokenManager;
        private readonly IDialogService _dialogService;
        private Dictionary<ReadingStatus, BookshelfItemDetails[]> bookshelves;
        public Dictionary<ReadingStatus, BookshelfItemDetails[]> Bookshelves
        {
            get => bookshelves;
            set
            {
                Set(ref bookshelves, value);
                RaisePropertyChanged("Reading");
                RaisePropertyChanged("Read");
            }
        }

        public BookshelfItemDetails[] Reading => bookshelves[ReadingStatus.Reading];
        public BookshelfItemDetails[] Read => bookshelves[ReadingStatus.Read];

        public ICommand ViewDetailsCommand { get; }
        public ICommand ReadingBookkActionsCommand { get; }
        public ICommand ReadBookActionsCommand { get; }

        public BookshelvesPageViewModel(IBookService bookService, IBookRepository repository, IAuthenticationTokenManager authenticationTokenManager, IDialogService dialogService)
        {
            this.bookService = bookService;
            this.repository = repository;
            this.authenticationTokenManager = authenticationTokenManager;
            _dialogService = dialogService;
            ViewDetailsCommand = new AsyncCommand<string>(ViewDetailsAsync);
            ReadingBookkActionsCommand = new AsyncCommand(ReadingBookActionsAsync);
            ReadBookActionsCommand = new AsyncCommand(ReadBookActionsAsync);
        }

        private async Task ReadBookActionsAsync()
        {
            string[] actions = new string[] { EnumDescriptions.BookActions[BookAction.WriteReview], EnumDescriptions.BookActions[BookAction.StartReading], EnumDescriptions.BookActions[BookAction.RecommendBook] };
            await Shell.Current.DisplayActionSheet("What do you want to do?", "Cancel", null, actions);
        }

        private Task ReadingBookActionsAsync()
        {
            throw new NotImplementedException();
        }

        private async Task ViewDetailsAsync(string bookId)
        {
            await Navigation.NavigateToAsync($"Details?bookId={bookId}");
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            await Task.WhenAny(LoadBooksFromCache(), LoadBooksFromServer()).ConfigureAwait(false);
        }

        public async Task<bool> LoadBooksFromCache()
        {
            var localBookshelves = new Dictionary<ReadingStatus, BookshelfItemDetails[]>();
            try
            {
                IsBusy = true;
                var bookItems = await repository.GetAllBooksAsync();

                var bookshelves = bookItems.GroupBy(b => b.ReadingStatus,
                    b => b,
                    (status, books) => new UserBookshelf()
                    {
                        UserId = authenticationTokenManager.Current.UserId,
                        ReadingStatus = status,
                        Count = bookItems.Count(),
                        Items = books.ToArray()
                    });

                foreach (var bookshelf in bookshelves)
                {
                    localBookshelves.Add(bookshelf.ReadingStatus, bookshelf.Items);
                }

                Bookshelves = localBookshelves;
            }
            finally
            {
                IsBusy = false;
            }

            return Bookshelves?.Any() ?? false;
        }

        public async Task LoadBooksFromServer()
        {
            var serverBookshelves = new Dictionary<ReadingStatus, BookshelfItemDetails[]>();

            try
            {
                IsBusy = true;
                var result = await bookService.GetMyBookShelvesAsync().ConfigureAwait(false);
                foreach (var bookshelf in result)
                {
                    serverBookshelves.Add(bookshelf.ReadingStatus, bookshelf.Items);
                    await repository.UpdateBookItemsAsync(bookshelf.Items);
                }

                Bookshelves = serverBookshelves;
            }
            catch (ApiException apiError)
            {
                await MainThread.InvokeOnMainThreadAsync(async () => await OnApiError(apiError));
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
    }
}