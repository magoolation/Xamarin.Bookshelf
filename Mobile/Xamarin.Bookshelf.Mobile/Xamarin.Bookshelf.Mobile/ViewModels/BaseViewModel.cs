using Refit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TinyMvvm;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Xamarin.Bookshelf.Mobile.ViewModels
{
    public class BaseViewModel : ViewModelBase
    {                
        protected Task DisplayAlertAsync(string title, string message, string actionButton)
        {
            return MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.DisplayAlert(title, message, actionButton));
        }

        protected Task DisplayAlertAsync(string title, string message, string actionButton, string cancelButton)
        {
            return MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.DisplayAlert(title, message, actionButton, cancelButton));
        }

        protected Task OnApiError(ApiException error)
        {
            switch (error.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    {
                        return ReAuthenticateAsync();
                    }
                default:
                    return DisplayAlertAsync("Oops", "Something went wrong! Please try again later.", "OK");
            }
        }

        private async Task ReAuthenticateAsync()
        {
            await Shell.Current.GoToAsync("//Login");
        }
    }
}