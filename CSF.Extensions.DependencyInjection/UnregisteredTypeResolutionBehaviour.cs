namespace CSF.Extensions.DependencyInjection
{
    public enum UnregisteredTypeResolutionBehaviour
    {
        None = 0,

        AttemptToResolveTransient,

        AttemptToResolveScoped,

        AttemptToResolveSingleton
    }
}