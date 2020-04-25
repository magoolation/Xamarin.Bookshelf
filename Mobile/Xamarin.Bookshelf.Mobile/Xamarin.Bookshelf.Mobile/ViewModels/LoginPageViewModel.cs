using AsyncAwaitBestPractices.MVVM;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Bookshelf.Mobile.Models;
using Xamarin.Bookshelf.Mobile.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Xamarin.Bookshelf.Mobile.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        private readonly IAuthenticationManager authenticationManager;

        public ICommand LoginWithGoogleCommand { get; }
        public ICommand SigninWithAppleCommand { get; }

        public LoginPageViewModel(IAuthenticationManager authenticationManager)
        {
            this.authenticationManager = authenticationManager;
            LoginWithGoogleCommand = new AsyncCommand(LoginWithGoogleAsync);
            SigninWithAppleCommand = new AsyncCommand(SigninWithAppleAsync);
        }

        private async Task SigninWithAppleAsync()
        {
            try
            {
                await authenticationManager.SigninWithAppleAsync();

                await Shell.Current.GoToAsync("//Main");
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", ex.Message, "OK");
            }
        }

        private async Task LoginWithGoogleAsync()
        {
            try
            {
                await authenticationManager.LoginWithGoogle();

                MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync("//Main"));
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", ex.Message, "OK");
            }
        }
    }
}