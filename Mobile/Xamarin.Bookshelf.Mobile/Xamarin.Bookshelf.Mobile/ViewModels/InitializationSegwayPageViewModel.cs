using Xamarin.Bookshelf.Mobile.Services;
using Xamarin.Forms;

namespace Xamarin.Bookshelf.Mobile.ViewModels
{
    public class InitializationSegwayPageViewModel : BaseViewModel
    {
        private readonly IAuthenticationManager authenticationManager;
        private readonly BookService bookService;

        public InitializationSegwayPageViewModel(IAuthenticationManager authenticationManager, BookService bookService)
        {
            this.authenticationManager = authenticationManager;
            this.bookService = bookService;
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            await authenticationManager.RefreshAsync();

            if (!authenticationManager.IsAuthenticated)
            {
                await Shell.Current.GoToAsync("//Login");
                return;
            }
            await Shell.Current.GoToAsync("//Main");
        }
    }
}
