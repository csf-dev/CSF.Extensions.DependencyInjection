namespace Microsoft.Extensions.DependencyInjection
{
    public enum UnregisteredTypeResolutionBehaviour
    {
        None = 0,

        AttemptToResolveTransient,

        AttemptToResolveScoped,

        AttemptToResolveSingleton
    }
}