using System;
using TinyMvvm.IoC;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Xamarin.Bookshelf.Mobile
{
    public class MicrosoftDependencyResolver : IResolver
    {
        private IServiceProvider serviceProvider;

        public MicrosoftDependencyResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public T Resolve<T>()
        {
            return serviceProvider.GetService<T>();
        }

        public T Resolve<T>(string key)
        {
            var services = serviceProvider.GetServices<T>();
            return services.FirstOrDefault(s => s.GetType().Name == key);
        }

        public object Resolve(Type type)
        {
            return serviceProvider.GetService(type);
        }
    }
}