using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CSF.Extensions.DependencyInjection
{
    [TestFixture,Parallelizable]
    public class LazyServicesExtenderTests
    {
        [Test,AutoMoqData]
        public void AddLazyServiceDescriptorShouldAddAnAdditionalServiceDescriptorForLazyOfTheServiceType(LazyServicesExtender sut, [NoAutoProperties] ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IMyInterface, MyClass>();
            sut.AddLazyServiceDescriptor<IMyInterface>(serviceCollection, serviceCollection[0]);

            Assert.Multiple(() =>
            {
                Assert.That(serviceCollection, Has.One.Matches<ServiceDescriptor>(d => d.ServiceType == typeof(IMyInterface)), "Service for the original interface type is present");
                Assert.That(serviceCollection, Has.One.Matches<ServiceDescriptor>(d => d.ServiceType == typeof(Lazy<IMyInterface>)), "Service for a lazy of the interface type is present");
            });
        }

        [Test,AutoMoqData]
        public void AddLazyServiceDescriptorShouldNotAddAnAdditionalServiceDescriptorIfTheServiceTypeAlreadyHasALazyDescriptor(LazyServicesExtender sut, [NoAutoProperties] ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<MyClass>();
            serviceCollection.AddTransient(s => new Lazy<MyClass>(() => s.GetRequiredService<MyClass>()));
            sut.AddLazyServiceDescriptor<MyClass>(serviceCollection, serviceCollection[0]);

            Assert.That(serviceCollection, Has.Count.EqualTo(2), "The count of service descriptors should be unchanged");
        }

        [Test,AutoMoqData]
        public void AddLazyServiceDescriptorShouldAllowTheServiceProviderToResolveLazily(LazyServicesExtender sut, [NoAutoProperties] ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IMyInterface, MyClass>();
            sut.AddLazyServiceDescriptor<IMyInterface>(serviceCollection, serviceCollection[0]);
            var services = serviceCollection.BuildServiceProvider();

            Assert.That(() => services.GetService<Lazy<IMyInterface>>()?.Value, Is.InstanceOf<MyClass>());
        }

        [Test,AutoMoqData]
        public void AddLazyServiceDescriptorsShouldAddLazyDescriptorsForEverythingButALazyDescriptor(LazyServicesExtender sut, [NoAutoProperties] ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IMyInterface, MyClass>();
            serviceCollection.AddTransient<MyClass>();
            serviceCollection.AddTransient<IEnumerable<IMyInterface>>(s => new [] { s.GetRequiredService<IMyInterface>() });
            serviceCollection.AddTransient<Lazy<MyClass>>(s => new Lazy<MyClass>(() => s.GetRequiredService<MyClass>()));

            sut.AddLazyServiceDescriptors(serviceCollection);

            Assert.Multiple(() =>
            {
                Assert.That(serviceCollection, Has.One.Matches<ServiceDescriptor>(d => d.ServiceType == typeof(IMyInterface)), "IMyInterface present");
                Assert.That(serviceCollection, Has.One.Matches<ServiceDescriptor>(d => d.ServiceType == typeof(Lazy<IMyInterface>)), "Lazy<IMyInterface> present");
                Assert.That(serviceCollection, Has.One.Matches<ServiceDescriptor>(d => d.ServiceType == typeof(IEnumerable<IMyInterface>)), "IEnumerable<IMyInterface> present");
                Assert.That(serviceCollection, Has.One.Matches<ServiceDescriptor>(d => d.ServiceType == typeof(Lazy<IEnumerable<IMyInterface>>)), "Lazy<IEnumerable<IMyInterface>> present");
                Assert.That(serviceCollection, Has.One.Matches<ServiceDescriptor>(d => d.ServiceType == typeof(MyClass)), "MyClass present");
                Assert.That(serviceCollection, Has.One.Matches<ServiceDescriptor>(d => d.ServiceType == typeof(Lazy<MyClass>)), "Lazy<MyClass> present");
                Assert.That(serviceCollection.Count, Is.EqualTo(6), "6 service descriptors present");
            });
        }

        public class MyClass : IMyInterface {}

        public interface IMyInterface {}
    }
}