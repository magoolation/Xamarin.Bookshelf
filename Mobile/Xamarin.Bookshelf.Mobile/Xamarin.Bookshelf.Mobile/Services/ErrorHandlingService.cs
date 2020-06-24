using Refit;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TinyNavigationHelper;

namespace Xamarin.Bookshelf.Mobile.Services
{
    public class ErrorHandlingService : IErrorHandlingService
    {
        private readonly INavigationHelper _navigation;
        private readonly IDialogService _dialogService;

        public ErrorHandlingService(INavigationHelper navigation, IDialogService dialogService)
        {
            _navigation = navigation;
            _dialogService = dialogService;
        }

        public Task HandleApiExceptionAsync(ApiException ex)
        {
            switch (ex.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    {
                        return ReAuthenticateAsync();
                    }
                default:
                    return _dialogService.DisplayAlertAsync("Oops", "Something went wrong! Please try again later.", "OK");
            }
        }

        private async Task ReAuthenticateAsync()
        {
            await _navigation.NavigateToAsync("//Login");
        }    

        public Task HandleExceptionAsync(Exception ex)
        {
            return _dialogService.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }
}
