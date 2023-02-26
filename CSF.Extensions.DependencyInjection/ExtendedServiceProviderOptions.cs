using Microsoft.Extensions.DependencyInjection;

namespace CSF.Extensions.DependencyInjection
{
    public class ExtendedServiceProviderOptions : ServiceProviderOptions
    {
        public bool AddLazyResolvers { get; set; }

        public UnregisteredTypeResolutionBehaviour UnregisteredTypeResolutionBehaviour { get; set; }
    }
}