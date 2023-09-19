using System;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Extensions.DependencyInjection.UnregisteredTypes
{
    public class UnregisteredTypeResolvingServiceProviderDecorator : IServiceProvider, IServiceProviderIsService, ISupportRequiredService
    {
        readonly IServiceProvider wrapped;
        readonly IResolvesUnregisteredType resolver;

        public object GetRequiredService(Type serviceType)
        {
            if (IsService(serviceType))
                return wrapped.GetRequiredService(serviceType);

            return resolver.Resolve(serviceType);
        }

        public object GetService(Type serviceType)
        {
            if (IsService(serviceType))
                return wrapped.GetService(serviceType);

            return resolver.Resolve(serviceType);
        }

        public bool IsService(Type serviceType)
        {
            if (serviceType is null)
                throw new ArgumentNullException(nameof(serviceType));
            
            return wrapped is IServiceProviderIsService isService
                && isService.IsService(serviceType);
        }

        public UnregisteredTypeResolvingServiceProviderDecorator(IServiceProvider wrapped, IResolvesUnregisteredType resolver)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }
    }
}