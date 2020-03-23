using Xamarin.Forms;

[assembly: ExportFont("fa-regular-400.ttf")]

namespace Xamarin.Bookshelf.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
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
