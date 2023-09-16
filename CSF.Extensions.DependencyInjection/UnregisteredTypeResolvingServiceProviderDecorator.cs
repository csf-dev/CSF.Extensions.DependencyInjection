using System;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Extensions.DependencyInjection
{
    public class UnregisteredTypeResolvingServiceProviderDecorator : IServiceProvider, IServiceProviderIsService, ISupportRequiredService, IServiceScope
    {
        readonly IServiceProvider wrapped;
        readonly UnregisteredTypeResolutionBehaviour behaviour;

        public IServiceProvider ServiceProvider => this;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public object GetRequiredService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public bool IsService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public UnregisteredTypeResolvingServiceProviderDecorator(IServiceProvider wrapped, UnregisteredTypeResolutionBehaviour behaviour)
        {
            this.wrapped = wrapped;
            this.behaviour = behaviour;
        }
    }
}