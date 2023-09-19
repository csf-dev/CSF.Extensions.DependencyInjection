using System;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Extensions.DependencyInjection.UnregisteredTypes
{
    public class UnregisteredTypeResolvingServiceScopeDecorator : IServiceScope
    {
        readonly IServiceScope wrapped;
        readonly IResolvesUnregisteredType resolver;

        public IServiceProvider ServiceProvider { get; }

        public void Dispose()
        {
            if (resolver.Lifetime == ServiceLifetime.Scoped && resolver is IDisposable disposable)
                disposable.Dispose();

            wrapped.Dispose();
            GC.SuppressFinalize(this);
        }

        public UnregisteredTypeResolvingServiceScopeDecorator(IServiceScope wrapped, IResolvesUnregisteredType resolver)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            ServiceProvider = new UnregisteredTypeResolvingServiceProviderDecorator(wrapped.ServiceProvider, resolver);
        }
    }
}