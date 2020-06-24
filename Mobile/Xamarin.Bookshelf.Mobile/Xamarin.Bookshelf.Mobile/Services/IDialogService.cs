using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Bookshelf.Mobile.Services
{
    public interface IDialogService
    {
        Task DisplayAlertAsync(string title, string message, string accept);
        Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel);
        Task<string> DisplayActionSheet(string title, string cancel, string destruction, string[] actions);
    }
}
