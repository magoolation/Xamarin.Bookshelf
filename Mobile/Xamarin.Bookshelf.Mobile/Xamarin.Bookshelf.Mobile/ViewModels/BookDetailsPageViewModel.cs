using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;
using Refit;
using System;
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
    [QueryProperty("BookId", "bookId")]
    [QueryProperty("BookshelfItemId", "bookshelfItemId")]
    public class BookDetailsPageViewModel : BaseViewModel
    {
        private readonly IBookService bookService;
        private readonly IBookRepository repository;
        private readonly IAuthenticationTokenManager authenticationTokenManager;

        public ICommand AddToLibraryCommand { get; }
        public ICommand ReviewBookCommand { get; }

        public BookDetailsPageViewModel(IBookService bookService, IBookRepository repository, IAuthenticationTokenManager authenticationTokenManager)
        {
            this.bookService = bookService;
            this.repository = repository;
            this.authenticationTokenManager = authenticationTokenManager;

            AddToLibraryCommand = new AsyncCommand(AddToLibraryAsync);
            ReviewBookCommand = new AsyncCommand(ReviewBookAsync);
        }

        private async Task AddToLibraryAsync()
        {
            string result = await Shell.Current.DisplayActionSheet("Select a Bookshelf", "Cancel", null, EnumDescriptions.ReadingStatuses.Values.ToArray());
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


                await DisplayAlertAsync("Success", "Book added to your Bookshelf successfully!", "OK");
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

        private async Task ReviewBookAsync()
        {
            await Shell.Current.GoToAsync($"ReviewBook?bookId={BookId}");
        }

        public string BookId { get; set; }
        public string BookItemId { get; set; }

        private BookshelfItemDetails book;
        public BookshelfItemDetails Book
        {
            get => book;
            set => SetProperty(ref book, value);
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            await GetBookDetailsAsync().ConfigureAwait(false);
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
                await OnApiError(apiError);
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
