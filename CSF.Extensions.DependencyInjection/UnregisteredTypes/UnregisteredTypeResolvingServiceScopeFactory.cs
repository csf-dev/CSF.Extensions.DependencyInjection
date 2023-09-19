using System;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Extensions.DependencyInjection.UnregisteredTypes
{
    public class UnregisteredTypeResolvingServiceScopeFactory : IServiceScopeFactory
    {
        readonly ServiceProvider serviceProvider;

        public IServiceScope CreateScope()
        {
            var innerScope = serviceProvider.CreateScope();
        }

        public UnregisteredTypeResolvingServiceScopeFactory(ServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
    }
}