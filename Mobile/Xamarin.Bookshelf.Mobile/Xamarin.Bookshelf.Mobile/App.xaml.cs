using Xamarin.Bookshelf.Mobile.Views;
using Xamarin.Forms;


namespace Xamarin.Bookshelf.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var flags = new string[] { "CAROUSSELVIEW_EXPERIMENTAL", "SWIPEVIEW_EXPERIMENTAL" };
            Device.SetFlags(flags);
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
