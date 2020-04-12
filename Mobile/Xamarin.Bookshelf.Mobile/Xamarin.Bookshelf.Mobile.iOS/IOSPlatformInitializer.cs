using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UIKit;
using Xamarin.Bookshelf.Mobile.Helpers;

namespace Xamarin.Bookshelf.Mobile.iOS
{
    public class IOSPlatformInitializer : IPlatformInitializer
    {
        public void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
        }
    }
}