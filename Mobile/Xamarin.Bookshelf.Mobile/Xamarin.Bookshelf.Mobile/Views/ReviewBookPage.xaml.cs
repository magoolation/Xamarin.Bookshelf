using Xamarin.Bookshelf.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.Bookshelf.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewBookPage : ContentPage
    {
        public ReviewBookPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is BaseViewModel vm)
                {
                vm.OnAppearing();
            }
        }
    }
}