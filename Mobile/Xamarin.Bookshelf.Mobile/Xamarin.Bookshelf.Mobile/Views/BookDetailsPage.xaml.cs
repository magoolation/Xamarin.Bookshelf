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
    public partial class BookDetailsPage : ContentPage
    {
        public BookDetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((BookDetailsPageViewModel)BindingContext).OnAppearing();
        }

        protected override void OnDisappearing()
        {
            ((BaseViewModel)BindingContext).OnDisappearing();
        }
    }
}