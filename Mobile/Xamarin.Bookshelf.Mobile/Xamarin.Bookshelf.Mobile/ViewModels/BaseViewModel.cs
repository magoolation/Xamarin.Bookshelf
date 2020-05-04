using Refit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Xamarin.Bookshelf.Mobile.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void OnAppearing() { }
        public virtual void OnDisappearing() { }

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