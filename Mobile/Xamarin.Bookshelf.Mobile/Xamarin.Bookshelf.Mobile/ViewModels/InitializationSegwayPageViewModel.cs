using System.Threading.Tasks;
using Xamarin.Bookshelf.Mobile.Services;
using Xamarin.Forms;

namespace Xamarin.Bookshelf.Mobile.ViewModels
{
    public class InitializationSegwayPageViewModel : BaseViewModel
    {
        private readonly IAuthenticationManager authenticationManager;

        public InitializationSegwayPageViewModel(IAuthenticationManager authenticationManager)
        {
            this.authenticationManager = authenticationManager;
        }
        
        public override async void OnAppearing()
        {
            base.OnAppearing();

            await authenticationManager.RefreshAsync();

            if (authenticationManager.Current.IsAuthenticated)
            {
                await Shell.Current.GoToAsync("//Main");
            }
            else
            {
            await Shell.Current.GoToAsync("//Login");
            }
        }
    }
}
