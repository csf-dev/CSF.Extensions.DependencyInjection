using System;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Extensions.DependencyInjection.UnregisteredTypes
{
    /// <summary>
    /// Implementation of <see cref="IResolvesUnregisteredType"/> which provides no caching; only type creation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The methods of this type provide no caching, so for example <see cref="Contains(ServiceDescriptor)"/> and
    /// <see cref="TryGetCached(ServiceDescriptor, out object)"/> will always return <see langword="false" />.
    /// <see cref="Lifetime"/> will always return <see cref="ServiceLifetime.Transient"/> and <see cref="Resolve(ServiceDescriptor)"/>
    /// will always create a new instance.
    /// </para>
    /// </remarks>
    public class UnregisteredTypeResolver : IResolvesUnregisteredType
    {
        /// <inheritdoc />
        public ServiceLifetime Lifetime => ServiceLifetime.Transient;

        /// <inheritdoc />
        public bool Contains(Type serviceType) => false;

        /// <inheritdoc />
        public object Resolve(Type serviceType)
        {
            if (serviceType is null)
                throw new ArgumentNullException(nameof(serviceType));

            if (serviceType.IsAbstract)
                throw new InvalidOperationException($"The specified service type {serviceType.FullName} is unregistered but cannot be resolved because it is an abstract type.");
            if (serviceType.GetConstructor(Type.EmptyTypes) is null)
                throw new InvalidOperationException($"The specified service type {serviceType.FullName} is unregistered but cannot be resolved because it does not have a parameterless constructor.");

            return Activator.CreateInstance(serviceType);
        }

        /// <inheritdoc />
        public bool TryGetCached(Type serviceType, out object value)
        {
            value = null;
            return false;
        }
    }
}