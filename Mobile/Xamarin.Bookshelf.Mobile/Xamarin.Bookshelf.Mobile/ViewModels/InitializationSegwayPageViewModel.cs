using System.Threading.Tasks;
using Xamarin.Bookshelf.Mobile.Services;
using Xamarin.Bookshelf.Shared.Services;

namespace Xamarin.Bookshelf.Mobile.ViewModels
{
    public class InitializationSegwayPageViewModel : BaseViewModel
    {
        private readonly IAuthenticationManager authenticationManager;
        private readonly IBookService bookService;

        public InitializationSegwayPageViewModel(IAuthenticationManager authenticationManager, IBookService bookService)
        {
            this.authenticationManager = authenticationManager;
            this.bookService = bookService;
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            await authenticationManager.RefreshAsync().ConfigureAwait(false);

            if (!authenticationManager.IsAuthenticated)
            {
                await Navigation.NavigateToAsync("//Login");
            }
            else
            {
                await Navigation.NavigateToAsync("//Main");
            }
        }
    }
}
