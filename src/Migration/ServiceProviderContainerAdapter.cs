using System;
using Microsoft.Extensions.DependencyInjection;

namespace RapidCore.Mongo.Migration
{
    /// <summary>
    /// Container adapter for <see cref="IServiceProvider"/>
    /// </summary>
    public class ServiceProviderContainerAdapter : IContainerAdapter
    {
        private readonly IServiceProvider serviceProvider;

        public ServiceProviderContainerAdapter(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public T Resolve<T>()
        {
            return serviceProvider.GetService<T>();
        }

        public T Resolve<T>(string name)
        {
            // named instances does not exist in IServiceProvider
            // so just do a normal resolve
            return Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return serviceProvider.GetService(type);
        }
    }
}