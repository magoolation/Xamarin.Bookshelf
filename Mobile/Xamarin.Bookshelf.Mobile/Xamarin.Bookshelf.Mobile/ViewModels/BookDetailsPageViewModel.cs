using AsyncAwaitBestPractices.MVVM;
using LiteDB;
using Refit;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Bookshelf.Mobile.Services;
using Xamarin.Bookshelf.Shared;
using Xamarin.Bookshelf.Shared.Models;
using Xamarin.Bookshelf.Shared.Services;

namespace Xamarin.Bookshelf.Mobile.ViewModels
{
    public class BookDetailsPageViewModel : BaseViewModel
    {
        private readonly IBookService bookService;
        private readonly IBookRepository repository;
        private readonly IAuthenticationTokenManager authenticationTokenManager;
        private readonly IDialogService _dialogService;
        private readonly IErrorHandlingService _errorHandlingService;

        public ICommand AddToLibraryCommand { get; }
        public ICommand ReviewBookCommand { get; }

        public BookDetailsPageViewModel(IBookService bookService, IBookRepository repository, IAuthenticationTokenManager authenticationTokenManager, IDialogService dialogService, IErrorHandlingService errorHandlingService)
        {
            this.bookService = bookService;
            this.repository = repository;
            this.authenticationTokenManager = authenticationTokenManager;
            _dialogService = dialogService;
            _errorHandlingService = errorHandlingService;
            AddToLibraryCommand = new AsyncCommand(AddToLibraryAsync);
            ReviewBookCommand = new AsyncCommand(ReviewBookAsync);
        }

        private async Task AddToLibraryAsync()
        {
            string result = await _dialogService.DisplayActionSheet("Select a Bookshelf", "Cancel", null, EnumDescriptions.ReadingStatuses.Values.ToArray());
            if (!string.IsNullOrWhiteSpace(result) && result != "Cancel")
            {
                await RegisterBookAsync(result);
            }
        }

        private async Task RegisterBookAsync(string selected)
        {
            try
            {
                IsBusy = true;
                ReadingStatus status = selected == EnumDescriptions.ReadingStatuses[ReadingStatus.Reading]
                    ? ReadingStatus.Reading
                    : selected == EnumDescriptions.ReadingStatuses[ReadingStatus.Read] ? ReadingStatus.Read : ReadingStatus.WantToRead;
                var bookshelf = new BookRegistration()
                {
                    Id = Guid.NewGuid().ToString(),
                    BookId = BookId,
                    ReadingStatus = status,
                };

                await bookService.RegisterBookAsync(bookshelf).ConfigureAwait(false);

                await repository.AddBookAsync(book);

                await _dialogService.DisplayAlertAsync("Success", "Book added to your Bookshelf successfully!", "OK");
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

        private async Task ReviewBookAsync()
        {
            await Navigation.NavigateToAsync($"ReviewBook?bookId={BookId}");
        }

        private string bookId;
        public string BookId
        {
            get => bookId;
            set
            {
                if (value != null && value != bookId)
                {
                    bookId = value;
                }
            }
        }

        private BookshelfItemDetails book;
        public BookshelfItemDetails Book
        {
            get => book;
            set => Set(ref book, value);
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            BookId = QueryParameters[nameof(bookId)];

            if (!string.IsNullOrWhiteSpace(BookId))
            {
                await GetBookDetailsAsync();
            }
        }

        private async Task GetBookDetailsAsync()
        {
            try
            {
                IsBusy = true;
                var result = await bookService.GetBookDetailsAsync(BookId).ConfigureAwait(false);
                Book = ConvertToBookshelfItemDetails(result);
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

        private BookshelfItemDetails ConvertToBookshelfItemDetails(BookDetails result)
        {
            return new BookshelfItemDetails()
            {
                BookId = result.BookId,
                Title = result.Title,
                SubTitle = result.SubTitle,
                Summary = result.Summary,
                PageCount = result.PageCount,
                Authors = result.Authors
            };
        }
    }
}
