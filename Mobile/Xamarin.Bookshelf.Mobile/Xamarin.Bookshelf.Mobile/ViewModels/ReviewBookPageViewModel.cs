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
    public class ReviewBookPageViewModel : BaseViewModel
    {
        private readonly IBookService bookService;
        private readonly IAuthenticationTokenManager authenticationTokenManager;

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

        private string reviewTitle; //R:18 G:140 B:250 
        public string ReviewTitle
        {
            get => reviewTitle;
            set => Set(ref reviewTitle, value);
        }

        private string review;
        public string Review
        {
            get => review;
            set => Set(ref review, value);
        }

        private double rating;
        public double Rating
        {
            get => rating;
            set => Set(ref rating, value);
        }

        public ICommand SendCommand { get; }
        public ICommand CancelCommand { get; }

        public ReviewBookPageViewModel(IBookService bookService, IAuthenticationTokenManager authenticationTokenManager)
        {
            this.bookService = bookService;
            SendCommand = new AsyncCommand(SendReviewAsync);
            CancelCommand = new AsyncCommand(CancelAsync);
            this.authenticationTokenManager = authenticationTokenManager;
        }

        private async Task CancelAsync()
        {
            await Navigation.NavigateToAsync($"..?bookId={BookId}");
        }

        private async Task SendReviewAsync()
        {
            try
            {
                var bookReview = new BookReviewRegistration()
                {
                    BookId = BookId,
                    Rating = rating,
                    Title = reviewTitle,
                    Review = review
                };

                await bookService.ReviewBookAsync(bookReview).ConfigureAwait(false);
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await DisplayAlertAsync("Success", "Your review was received successfully. Thank you!", "OK");
                    await Shell.Current.GoToAsync("..");
                });
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", ex.Message, "OK");
            }
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            BookId = QueryParameters[nameof(bookId)];
            ReviewTitle = string.Empty;
            Review = string.Empty;
            Rating = 0;
        }
    }
}
