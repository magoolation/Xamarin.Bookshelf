using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Bookshelf.Shared.Models;
using Xamarin.Bookshelf.Shared.Services;
using Xamarin.Forms;

namespace Xamarin.Bookshelf.Mobile.ViewModels
{
    public class BookshelvesPageViewModel : BaseViewModel
    {
        private readonly IBookService bookService;

        public ICommand ViewDetailsCommand { get; }
        public BookshelvesPageViewModel(IBookService bookService)
        {
            this.bookService = bookService;
            ViewDetailsCommand = new AsyncCommand<string>(ViewDetailsAsync);
        }

        private async Task ViewDetailsAsync(string bookId)
        {
            await Shell.Current.GoToAsync($"Details?bookId={bookId}");
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            var serverBookshelves = new Dictionary<ReadingStatus, Book[]>();

            var result = await bookService.GetUserBookShelvesAsync("magoolation@me.com");
            foreach(var bookshelf in result)
            {
                serverBookshelves.Add(bookshelf.ReadingStatus, bookshelf.Books);
            }
        }
    }
}
