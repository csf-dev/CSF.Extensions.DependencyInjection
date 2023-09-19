using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Extensions.DependencyInjection.UnregisteredTypes
{
    /// <summary>
    /// Implementation of <see cref="IResolvesUnregisteredType"/> which provides caching.
    /// </summary>
    public class ResolvedUnregisteredTypeCache : IResolvesUnregisteredType, IDisposable
    {
        readonly ConcurrentDictionary<Type, object> cache = new ConcurrentDictionary<Type, object>();
        readonly UnregisteredTypeResolver resolver = new UnregisteredTypeResolver();
        bool disposed;

        /// <inheritdoc />
        public ServiceLifetime Lifetime { get; }

        /// <inheritdoc />
        public bool TryGetCached(Type serviceType, out object value)
        {
            if (serviceType is null)
                throw new ArgumentNullException(nameof(serviceType));
            if (disposed)
                throw new ObjectDisposedException(nameof(ResolvedUnregisteredTypeCache));

            return cache.TryGetValue(serviceType, out value);
        }

        /// <inheritdoc />
        public object Resolve(Type serviceType)
        {
            if (serviceType is null)
                throw new ArgumentNullException(nameof(serviceType));
            if (disposed)
                throw new ObjectDisposedException(nameof(ResolvedUnregisteredTypeCache));

            return cache.GetOrAdd(serviceType, resolver.Resolve);
        }

        /// <inheritdoc />
        public bool Contains(Type serviceType)
        {
            if (serviceType is null)
                throw new ArgumentNullException(nameof(serviceType));
            if (disposed)
                throw new ObjectDisposedException(nameof(ResolvedUnregisteredTypeCache));

            return cache.ContainsKey(serviceType);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            foreach(var service in cache.Values)
            {
                if (service is IDisposable disposable)
                    disposable.Dispose();
            }
            GC.SuppressFinalize(this);
            disposed = true;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ResolvedUnregisteredTypeCache"/>
        /// </summary>
        /// <param name="lifetime">The lifetime which should be inherited for any newly-resolved unregistered services.</param>
        /// <exception cref="ArgumentException">If the <paramref name="lifetime"/> is not either <see cref="ServiceLifetime.Scoped"/> or <see cref="ServiceLifetime.Singleton"/>.</exception>
        public ResolvedUnregisteredTypeCache(ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            Lifetime = lifetime switch
            {
                ServiceLifetime.Singleton or ServiceLifetime.Scoped => lifetime,
                _ => throw new ArgumentException($"The lifetime must be either {nameof(ServiceLifetime.Singleton)} or {nameof(ServiceLifetime.Scoped)}.", nameof(lifetime)),
            };
        }
    }
}