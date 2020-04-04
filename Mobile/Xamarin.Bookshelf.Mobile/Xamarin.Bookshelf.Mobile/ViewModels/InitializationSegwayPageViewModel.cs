using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Bookshelf.Mobile.ViewModels
{
    public class InitializationSegwayPageViewModel: BaseViewModel
    {
        public override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(5000);
            await Shell.Current.GoToAsync("//Login");
        }
    }
}
