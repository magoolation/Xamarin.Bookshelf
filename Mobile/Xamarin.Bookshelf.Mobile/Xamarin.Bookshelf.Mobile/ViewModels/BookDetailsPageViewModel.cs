using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Bookshelf.Shared.Models;
using Xamarin.Bookshelf.Shared.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using ABookshelf = Xamarin.Bookshelf.Shared.Models.Bookshelf;

namespace Xamarin.Bookshelf.Mobile.ViewModels
{
    [QueryProperty("BookId", "bookId")]
    public class BookDetailsPageViewModel : BaseViewModel
    {
        private readonly IBookService bookService;

        public ICommand AddToLibraryCommand { get; }
        public ICommand ReviewBookCommand { get; }

        public BookDetailsPageViewModel(IBookService bookService)
        {
            this.bookService = bookService;

            AddToLibraryCommand = new Command(AddToLibrary);
            ReviewBookCommand = new Command(ReviewBook);
        }

        private async void AddToLibrary(object obj)
        {
            string result = await Shell.Current.DisplayActionSheet("Select a Bookshelf", "Cancel", null, new string[] { "I want to read", "I'm reading", "I already read" });
            if (!string.IsNullOrWhiteSpace(result) && result != "Cancel")
            {
                await RegisterBookAsync(result);
            }
        }

        private async Task RegisterBookAsync(string selected)
        {
            try
            {
                ReadingStatus status;
                if (selected== "I am reading")
                {
                    status = ReadingStatus.Reading;
                }
                else if (selected == "I already read")
                {
                    status = ReadingStatus.Read;
                }
                else
                {
                    status = ReadingStatus.WantToRead;
                }

                IsBusy = true;
            var bookshelf = new ABookshelf()
            {
                BookId = BookId,
                ReadingStatus = status,
                UserId = "magoolation@me.com",
                CreatedAt = DateTimeOffset.Now
        };

                await bookService.RegisterBookAsync(bookshelf).ConfigureAwait(false);
                MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.DisplayAlert("Success", "Book added to your Bookshelf successfully!", "OK"));
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

        private async void ReviewBook(object obj)
{
            await Shell.Current.GoToAsync($"//ReviewBook?bookId={BookId}");
}

public string BookId { get; set; }

private Book book;
public Book Book
{
    get => book;
    set => SetProperty(ref book, value);
}

public override async void OnAppearing()
{
    base.OnAppearing();

    await GetBookDetailsAsync();
}

public override void OnDisappearing()
{
    Book = null;
}

private async Task GetBookDetailsAsync()
{
    try
    {
        IsBusy = true;
        var result = await bookService.GetBookDetailsAsync(BookId).ConfigureAwait(false);
        Book = result;
    }
    catch (Exception ex)
    {
        await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
    }
    finally
    {
        IsBusy = false;
    }
}
    }
}
