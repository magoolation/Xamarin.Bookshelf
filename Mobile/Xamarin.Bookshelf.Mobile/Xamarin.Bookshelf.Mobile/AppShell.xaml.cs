using System;
using System.Collections.Generic;
using Xamarin.Bookshelf.Mobile.Views;
using Xamarin.Forms;

namespace Xamarin.Bookshelf.Mobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("MyBooks/Details", typeof(BookDetailsPage));
            Routing.RegisterRoute("SearchBooks/Details", typeof(BookDetailsPage));
            Routing.RegisterRoute("MyBooks/Details/ReviewBook", typeof(ReviewBookPage));
            Routing.RegisterRoute("SearchBooks/Details/ReviewBook", typeof(ReviewBookPage));
        }
    }
}
