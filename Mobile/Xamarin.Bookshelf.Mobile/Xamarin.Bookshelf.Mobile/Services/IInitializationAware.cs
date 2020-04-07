using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Bookshelf.Mobile.Services
{
    public interface IInitializationAware
    {
        Task OnInitializedAsync(Dictionary<string, object> parameters);
    }
}
