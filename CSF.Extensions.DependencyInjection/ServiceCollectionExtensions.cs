using System;
using CSF.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceProvider BuildServiceProvider(this ServiceCollection serviceCollection, ExtendedServiceProviderOptions options)
        {
            if (serviceCollection is null)
                throw new ArgumentNullException(nameof(serviceCollection));

            if(options.AddLazyResolvers)
                new LazyServicesExtender().AddLazyServiceDescriptors(serviceCollection);

            return serviceCollection.BuildServiceProvider((ServiceProviderOptions) options);
        }
    }
}