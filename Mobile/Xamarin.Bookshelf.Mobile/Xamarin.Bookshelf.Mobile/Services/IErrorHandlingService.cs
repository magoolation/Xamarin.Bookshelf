using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Bookshelf.Mobile.Services
{
    public interface IErrorHandlingService
    {
        Task HandleExceptionAsync(Exception ex);
        Task HandleApiExceptionAsync(ApiException ex);
    }
}
