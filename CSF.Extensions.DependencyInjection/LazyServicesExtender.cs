using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Extensions.DependencyInjection
{
    /// <summary>
    /// An object which extends the registrations present within a <see cref="ServiceCollection"/> by adding lazy resolvers for all
    /// registered services.
    /// </summary>
    public class LazyServicesExtender
    {
        static readonly MethodInfo addResolverGeneric = typeof(LazyServicesExtender).GetMethod(nameof(AddLazyServiceDescriptor));

        /// <summary>
        /// Adds lazy resolvers for every registered service within the specified service collection.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method iterates through every <see cref="ServiceDescriptor"/> within the service collection.
        /// For any services where the service type is not already <see cref="Lazy{T}"/>, and where the
        /// service collection does not already have a registration for <see cref="Lazy{T}"/> of the same service
        /// type, it adds a new service descriptor.
        /// The new service descriptor is for a <see cref="Lazy{T}"/> of the same service type, which resolves
        /// the service from the original descriptor.
        /// </para>
        /// </remarks>
        /// <param name="serviceCollection">A service collection to extend with lazy service descriptors.</param>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        /// Conditionally adds a lazy resolver for the specified service descriptor, which exists within a specified service collection.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method does not add the lazy resolver (it exits with no effect) if the specified <paramref name="serviceCollection"/>
        /// already contains a descriptor where the serivce type is equal to <see cref="Lazy{T}"/> of the same generic type as
        /// <typeparamref name="T"/>.
        /// </para>
        /// <para>
        /// When adding a lazy service descriptor, the descriptor is added with the same <see cref="ServiceLifetime"/> as the original
        /// descriptor.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">The generic type of the desired lazy service.</typeparam>
        /// <param name="serviceCollection">The service collection, from which <paramref name="descriptor"/> is taken.</param>
        /// <param name="descriptor">A service descriptor, for which to add a lazy descriptor to the service collection, of the same generic type and service lifetime.</param>
        public void AddLazyServiceDescriptor<T>(ServiceCollection serviceCollection, ServiceDescriptor descriptor)
        {
            if (serviceCollection is null)
                throw new ArgumentNullException(nameof(serviceCollection));
            if (descriptor is null)
                throw new ArgumentNullException(nameof(descriptor));

            // Don't do anything if the service collection already contains a lazy registration for this service type
            if (serviceCollection.Any(x => x.ServiceType == typeof(Lazy<T>))) return;

            switch(descriptor.Lifetime)
            {
            case ServiceLifetime.Scoped:
                serviceCollection.AddScoped(services => new Lazy<T>(() => services.GetRequiredService<T>()));
                break;
            case ServiceLifetime.Singleton:
                serviceCollection.AddSingleton(services => new Lazy<T>(() => services.GetRequiredService<T>()));
                break;
            default:
                serviceCollection.AddTransient(services => new Lazy<T>(() => services.GetRequiredService<T>()));
                break;
            }
        }
            
    }
}