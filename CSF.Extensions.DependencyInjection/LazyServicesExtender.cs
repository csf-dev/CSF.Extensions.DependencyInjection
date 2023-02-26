using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Extensions.DependencyInjection
{
    public class LazyServicesExtender
    {
        static readonly MethodInfo addResolverGeneric = typeof(LazyServicesExtender).GetMethod(nameof(AddLazyServiceDescriptor));

        public void AddLazyServiceDescriptors(ServiceCollection serviceCollection)
        {
            if (serviceCollection is null)
                throw new ArgumentNullException(nameof(serviceCollection));

            var nonLazyDescriptors = serviceCollection
                .Where(x => !x.ServiceType.IsGenericType || x.ServiceType.GetGenericTypeDefinition() != typeof(Lazy<>))
                .ToList();

            foreach(var descriptor in nonLazyDescriptors)
                addResolverGeneric.MakeGenericMethod(descriptor.ServiceType).Invoke(this, new object[] { serviceCollection, descriptor });
        }

        public void AddLazyServiceDescriptor<T>(ServiceCollection serviceCollection, ServiceDescriptor descriptor)
        {
            if(serviceCollection.Any(x => x.ServiceType == typeof(Lazy<T>))) return;
            serviceCollection.AddTransient<Lazy<T>>(services => new Lazy<T>(() => services.GetRequiredService<T>()));
        }
            
    }
}