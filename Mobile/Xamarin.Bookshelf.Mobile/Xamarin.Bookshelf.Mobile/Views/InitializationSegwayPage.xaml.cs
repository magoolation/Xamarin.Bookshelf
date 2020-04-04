using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Bookshelf.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.Bookshelf.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InitializationSegwayPage : ContentPage
    {
        public InitializationSegwayPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ((BaseViewModel)BindingContext).OnAppearing();
        }
    }
}