using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Bookshelf.Mobile.Helpers
{
    public interface IPlatformInitializer
    {
        void ConfigureServices(HostBuilderContext context, IServiceCollection services);
    }
}
