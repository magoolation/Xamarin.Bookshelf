using Xamarin.Bookshelf.Mobile.Helpers;
using Xamarin.Forms;


namespace Xamarin.Bookshelf.Mobile
{
    public partial class App : Application
    {
        private readonly IPlatformInitializer platformInitializer;

        public App(IPlatformInitializer platformInitializer = null)
        {
            this.platformInitializer = platformInitializer;
            InitializeComponent();

            var flags = new string[] { "CAROUSSELVIEW_EXPERIMENTAL", "INDICATORVIEW_EXPERIMENTAL", "SWIPEVIEW_EXPERIMENTAL", "EXPANDERVIEW_EXPERIMENTAL" };
            Device.SetFlags(flags);                        
        }

        protected override async void OnStart()
        {
            Startup.Init(platformInitializer);

            MainPage = new AppShell();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
