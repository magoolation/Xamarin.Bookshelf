using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Bookshelf.Shared.Models;
using Xamarin.Bookshelf.Shared.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Xamarin.Bookshelf.Mobile.ViewModels
{
    public class BookshelvesPageViewModel : BaseViewModel
    {
        private readonly IBookService bookService;

        private Dictionary<ReadingStatus, Book[]> bookshelves;
        public Dictionary<ReadingStatus, Book[]> Bookshelves
        {
            get => bookshelves;
            set
            {
                SetProperty(ref bookshelves, value);
                OnPropertyChanged("WantToRead");
                OnPropertyChanged("Reading");
                OnPropertyChanged("Read");
            }
        }

        public Book[] WantToRead => bookshelves[ReadingStatus.WantToRead];
        public Book[] Read => bookshelves[ReadingStatus.Read];
        public Book[] Reading => bookshelves[ReadingStatus.Reading];

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

            try
            {
                var result = await bookService.GetUserBookShelvesAsync("magoolation@me.com").ConfigureAwait(false);
                foreach (var bookshelf in result)
                {
                    serverBookshelves.Add(bookshelf.ReadingStatus, bookshelf.Books);
                }

                Bookshelves = serverBookshelves;
            }
            catch (Exception ex)
            {                
            MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.DisplayAlert("Error", ex.Message, "OK"));
            }
        }
    }
}