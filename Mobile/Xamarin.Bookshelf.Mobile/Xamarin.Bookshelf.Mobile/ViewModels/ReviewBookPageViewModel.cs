using AsyncAwaitBestPractices.MVVM;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Bookshelf.Mobile.Services;
using Xamarin.Bookshelf.Shared.Models;
using Xamarin.Bookshelf.Shared.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Xamarin.Bookshelf.Mobile.ViewModels
{
    [QueryProperty("BookId", "bookId")]
    public class ReviewBookPageViewModel : BaseViewModel
    {
        private readonly IBookService bookService;
        private readonly IAuthenticationTokenManager authenticationTokenManager;

        public string BookId { get; set; }

        private string reviewTitle; //R:18 G:140 B:250 
        public string ReviewTitle
        {
            get => reviewTitle;
            set => SetProperty(ref reviewTitle, value);
        }

        private string review;
        public string Review
        {
            get => review;
            set => SetProperty(ref review, value);
        }

        private double rating;
        public double Rating
        {
            get => rating;
            set => SetProperty(ref rating, value);
        }

        public ICommand SendCommand { get; }
        public ICommand CancelCommand { get; }

        public ReviewBookPageViewModel(BookService bookService, IAuthenticationTokenManager authenticationTokenManager)
        {
            this.bookService = bookService.Endpoint;
            SendCommand = new AsyncCommand(SendReviewAsync);
            CancelCommand = new AsyncCommand(CancelAsync);
            this.authenticationTokenManager = authenticationTokenManager;
        }

        private async Task CancelAsync()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        private async Task SendReviewAsync()
        {
            try
            {
                var bookReview = new BookReview()
                {
                    BookId = BookId,
                    UserId = authenticationTokenManager.Current.UserId,
                    Rating = rating,
                    Title = reviewTitle,
                    Review = review
                };

                await bookService.ReviewBookAsync(bookReview).ConfigureAwait(false);
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.DisplayAlert("Success", "Your review was received successfully. Thank you!", "OK");
                    await Shell.Current.Navigation.PopModalAsync();
                });
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.DisplayAlert("Error", ex.Message, "OK"));
            }
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            ReviewTitle = string.Empty;
            Review = string.Empty;
            Rating = 0;
        }
    }
}
