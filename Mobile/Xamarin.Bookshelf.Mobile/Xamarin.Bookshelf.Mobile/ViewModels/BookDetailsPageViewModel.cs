using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Bookshelf.Shared.Models;
using Xamarin.Bookshelf.Shared.Services;
using Xamarin.Forms;

namespace Xamarin.Bookshelf.Mobile.ViewModels
{
    [QueryProperty("BookId", "bookId")]
    public class BookDetailsPageViewModel : BaseViewModel
    {
        private readonly IBookService bookService;

        public BookDetailsPageViewModel(IBookService bookService)
        {
            this.bookService = bookService;
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
