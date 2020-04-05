using Xamarin.Bookshelf.Mobile.Views;
using Xamarin.Forms;

namespace Xamarin.Bookshelf.Mobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("Details", typeof(BookDetailsPage));
            Routing.RegisterRoute("ReviewBook", typeof(ReviewBookPage));
        }
    }
}
