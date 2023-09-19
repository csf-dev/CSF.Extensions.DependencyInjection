using System;
using CSF.Extensions.DependencyInjection.Lazy;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for <see cref="ServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Builds a service provider using the extended service provider options available in CSF.Extensions.DependencyInjection.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="options">The serivce provider options.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">If any parameter value is <see langword="null" />.</exception>
        public static IServiceProvider BuildServiceProvider(this ServiceCollection serviceCollection, ExtendedServiceProviderOptions options)
        {
            if (serviceCollection is null)
                throw new ArgumentNullException(nameof(serviceCollection));
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            if (options.AddLazyResolvers)
                new LazyServicesExtender().AddLazyServiceDescriptors(serviceCollection);

            return serviceCollection.BuildServiceProvider((ServiceProviderOptions) options);
        }
    }
}