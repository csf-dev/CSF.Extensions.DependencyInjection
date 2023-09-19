using System;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Extensions.DependencyInjection.UnregisteredTypes
{
    /// <summary>
    /// An object which provides the resolution (and possibly caching) of unregistered types.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Unregistered types are those which have not been added to the dependency injection container (the <see cref="ServiceCollection"/>).
    /// If <see cref="ExtendedServiceProviderOptions.UnregisteredTypeResolutionBehaviour"/> is set to any value other than
    /// <see cref="UnregisteredTypeResolutionBehaviour.None"/> then the created container will gain some limited ability to resolve unregistered
    /// types regardless.
    /// </para>
    /// <para>
    /// The functionality of resolving unregistered types requires that the type have a parameterless constructor, which will be used.
    /// Depending upon the lifetime specified by the chosen <see cref="UnregisteredTypeResolutionBehaviour"/>, instances of such services
    /// might be created transiently, per-lifetime scope or as singletons.
    /// </para>
    /// </remarks>
    public interface IResolvesUnregisteredType
    {
        /// <summary>
        /// Gets the lifetime for which unregistered services, resolved by this instance, will be cached.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Depending upon the value of <see cref="ExtendedServiceProviderOptions.UnregisteredTypeResolutionBehaviour"/>, when
        /// unregistered services are resolved, they will automatically have a lifetime.  That default lifetime for unregistered
        /// services is stored in this property.
        /// </para>
        /// </remarks>
        ServiceLifetime Lifetime { get; }

        /// <summary>
        /// Attempts to get a cached service from the current <see cref="Lifetime"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Gets an instance of the requested service, but only if it can be returned from the cache of previously-resolved instances.
        /// If it is not contained within the cache then this method will return <see langword="false" /> and the <paramref name="value"/>
        /// will have an undefined value.
        /// </para>
        /// </remarks>
        /// <param name="serviceType">A type specifying the service to resolve.</param>
        /// <param name="value">If this method returns <see langword="true" /> then the value parameter will expose the resolved service.
        /// If the method returns <see langword="false" /> then the value of this parameter is undefined.</param>
        /// <returns><see langword="true" /> if the service existed in <see cref="Lifetime"/> cache and was returned without creating a
        /// new instance; <see langword="false" /> otherwise.</returns>
        bool TryGetCached (Type serviceType, out object value);

        /// <summary>
        /// Resolves the specified service from the current instance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This might involve returning a cached service instance, if the specified service has already been resolved at least once
        /// within the current <see cref="Lifetime"/>.  It might alternatively mean resolving a new instance of the service if
        /// either the specified service has never been resolved before in the current lifetime or if the lifetime is
        /// <see cref="ServiceLifetime.Transient"/>.
        /// </para>
        /// </remarks>
        /// <param name="serviceType">A type specifying the service to resolve.</param>
        /// <returns>An instance of the requested service.</returns>
        object Resolve (Type serviceType);

        /// <summary>
        /// Gets a value indicating whether or not the specified service exists within the <see cref="Lifetime"/> cache.
        /// </summary>
        /// <param name="serviceType">A type specifying the service to find in the cache.</param>
        /// <returns><see langword="true" /> if the service existed in <see cref="Lifetime"/> cache; <see langword="false" /> otherwise.</returns>
        bool Contains(Type serviceType);
    }
}
