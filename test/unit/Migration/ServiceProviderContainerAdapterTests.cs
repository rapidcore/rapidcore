using Microsoft.Extensions.DependencyInjection;
using RapidCore.Mongo.Migration;
using Xunit;

namespace RapidCore.Mongo.UnitTests.Migration
{
    public class ServiceProviderContainerAdapterTests
    {
        private readonly ServiceProviderContainerAdapter container;

        public ServiceProviderContainerAdapterTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<MyClass>();
            
            container = new ServiceProviderContainerAdapter(serviceCollection.BuildServiceProvider());
        }

        [Fact]
        public void Resolve_ReturnsInstanceOfRegisteredClass()
        {
            Assert.IsAssignableFrom<MyClass>(container.Resolve<MyClass>());
        }
        
        [Fact]
        public void Resolve_returnsNull_ifNotRegistered()
        {
            Assert.Null(container.Resolve<Unregistered>());
        }
        
        [Fact]
        public void Resolve_type_ReturnsInstanceOfRegisteredClass()
        {
            Assert.IsAssignableFrom<MyClass>(container.Resolve(typeof(MyClass)));
        }
        
        [Fact]
        public void Resolve_type_returnsNull_ifNotRegistered()
        {
            Assert.Null(container.Resolve(typeof(Unregistered)));
        }
        
        [Fact]
        public void Resolve_named_ReturnsInstanceOfRegisteredClass()
        {
            Assert.IsAssignableFrom<MyClass>(container.Resolve<MyClass>("hephey"));
        }
        
        [Fact]
        public void Resolve_named_returnsNull_ifNotRegistered()
        {
            Assert.Null(container.Resolve<Unregistered>("hephey"));
        }

        #region Test classes
        private class MyClass
        {
        }
        
        private class Unregistered
        {
            
        }
        #endregion
    }
}