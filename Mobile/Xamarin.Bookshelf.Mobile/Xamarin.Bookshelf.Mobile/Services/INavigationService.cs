using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Bookshelf.Mobile.Services
{
    public interface INavigationService
    {
        Task GoToAsync(string path, Dictionary<string, object> parameters = null);
        Task GoBackAsync(Dictionary<string, object> parameters = null, bool isModal = false);
    }
}
