using AsyncAwaitBestPractices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Bookshelf.Mobile.Services
{
    public class NavigationService : INavigationService
    {
        public async Task GoToAsync(string path, Dictionary<string, object> parameters = null)
        {
            await Shell.Current.GoToAsync(path);
            InitializeViewModel(parameters);
        }

        private void InitializeViewModel(Dictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                var page = (Shell.Current.CurrentItem as IShellSectionController).PresentedPage;
                if (page.BindingContext is IInitializationAware vm)
                {
                    vm?.OnInitializedAsync(parameters)?.SafeFireAndForget();
                }
            }
        }

        public async Task GoBackAsync(Dictionary<string, object> parameters = null, bool isModal = false)
        {
            if (!isModal)
            {
                await Shell.Current.Navigation.PopAsync();
            }
            else
            {
                await Shell.Current.Navigation.PopModalAsync();
            }

            InitializeViewModel(parameters);
        }
    }
}