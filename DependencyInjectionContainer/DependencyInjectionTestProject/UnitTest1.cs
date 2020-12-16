using NUnit.Framework;
using DependencyInjectionLibrary;
using System.Collections.Generic;

namespace DependencyInjectionTestProject
{
    public class Tests
    {
        



        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreationDependencyTest()
        {
            DependenciesConfiguration config = new DependenciesConfiguration();
            config.Register<IService, Service1>(DependencyLifetime.InstancePerDependency);
            DependencyProvider provider = new DependencyProvider(config);
            
            var first = provider.Resolve<IService>();
            
            Assert.IsNotNull(first);
        }

        [Test]
        public void ChechDependencyTypeTest()
        {
            DependenciesConfiguration config = new DependenciesConfiguration();
            config.Register<IService, Service1>(DependencyLifetime.InstancePerDependency);
            DependencyProvider provider = new DependencyProvider(config);

            var first = provider.Resolve<IService>();

            Assert.AreEqual(typeof(Service1), first.GetType());
        }

        [Test]
        public void CheckManyImplementationsTest()
        {
            DependenciesConfiguration config = new DependenciesConfiguration();
            config.Register<IService, Service1>(DependencyLifetime.InstancePerDependency);
            config.Register<IService, Service2>(DependencyLifetime.InstancePerDependency);
            DependencyProvider provider = new DependencyProvider(config);

            var first = provider.Resolve<IEnumerable<IService>>();

            Assert.IsNotNull(first);
        }

        [Test]
        public void CheckRecursiveImplementationsTest()
        {
            DependenciesConfiguration config = new DependenciesConfiguration();
            config.Register<IService, Service3>(DependencyLifetime.InstancePerDependency);
            config.Register<IService2, Service21>(DependencyLifetime.InstancePerDependency);
            DependencyProvider provider = new DependencyProvider(config);

            var first = provider.Resolve<IService>();

            Assert.IsNotNull((first as Service3)._service2);
        }

        [Test]
        public void CheckOpenTypeCreationTest()
        {
            DependenciesConfiguration config = new DependenciesConfiguration();
            config.Register(typeof(IOpenService<>), typeof(OpenService<>), DependencyLifetime.InstancePerDependency);
            DependencyProvider provider = new DependencyProvider(config);

            var first = provider.Resolve<IOpenService<int>>();

            Assert.IsNotNull(first);
        }
    }
}