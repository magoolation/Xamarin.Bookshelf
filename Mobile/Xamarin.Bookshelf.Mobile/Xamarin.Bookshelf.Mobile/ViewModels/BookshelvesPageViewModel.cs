using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private Dictionary<ReadingStatus, BookshelfItem[]> bookshelves;
        public Dictionary<ReadingStatus, BookshelfItem[]> Bookshelves
        {
            get => bookshelves;
            set
            {
                SetProperty(ref bookshelves, value);
                OnPropertyChanged("Reading");
                OnPropertyChanged("Read");
            }
        }

        public BookshelfItem[] Reading => bookshelves[ReadingStatus.Reading];
        public BookshelfItem[] Read => bookshelves[ReadingStatus.Read];

        public ICommand ViewDetailsCommand { get; }
        public ICommand ReadingBookkActionsCommand { get; }
        public ICommand ReadBookActionsCommand { get; }

        public BookshelvesPageViewModel(IBookService bookService, IBookRepository repository)
        {
            this.bookService = bookService;
            this.repository = repository;
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
            await Shell.Current.GoToAsync($"Details?bookId={bookId}");
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            if (! await LoadBooksFromCache())
            {
                await LoadBooksFromServer();
            }
        }

        public async Task<bool> LoadBooksFromCache()
        {
            var localBookshelves = new Dictionary<ReadingStatus, BookshelfItem[]>();
            try
            {
                IsBusy = true;
                var bookItems = await repository.GetAllBooksAsync("magoolation@me.com");

                var bookshelves = bookItems.GroupBy(b => b.ReadingStatus,
                    b => b,
                    (status, books) => new UserBookshelf()
                    {
                        UserId = "magoolation@me.com",
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
            var serverBookshelves = new Dictionary<ReadingStatus, BookshelfItem[]>();

            try
            {
                IsBusy = true;
                var result = await bookService.GetUserBookShelvesAsync("magoolation@me.com").ConfigureAwait(false);
                foreach (var bookshelf in result)
                {
                    serverBookshelves.Add(bookshelf.ReadingStatus, bookshelf.Items);
                    await repository.UpdateBookItemsAsync(bookshelf.Items);
                }

                Bookshelves = serverBookshelves;
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
    }
}