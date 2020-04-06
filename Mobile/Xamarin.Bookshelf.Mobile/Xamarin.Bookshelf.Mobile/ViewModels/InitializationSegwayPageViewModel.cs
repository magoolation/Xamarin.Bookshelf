using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Bookshelf.Mobile.ViewModels
{
    public class InitializationSegwayPageViewModel : BaseViewModel
    {
        public override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(5000);
            wait Shell.Current.GoToAsync("//Main");
        }
    }
}
