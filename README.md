# Dependency Injection extensions

This package adds capabilities to Microsoft.Extensions.DependencyInjection.
At present, the only functionality it adds is optional **Lazy resolvers** for registered services.

## Lazy resolvers

Lazy resolvers are a convenience for services which do not always need to be resolved, particularly those which have a performance cost to resolve.
By resolving a `Lazy<T>` for the service, instead of just `T`, if the service is never used (IE: `Lazy<T>.Value` is never accessed) then the service is never fully resolved.

When `ExtendedServiceProviderOptions.AddLazyResolver` is `true` (see below) then before building the service provider, every service descriptor in the service collection is duplicated by an equivalent service descriptor for a lazy service of the same kind.

* Existing service descriptors that are already for `Lazy<T>` will not be used to generate lazy resolvers
* If there is already a matching service descriptor for `Lazy<T>` then a new one will not be generated
* Service lifetime scope is respected in the generation of lazy resolvers
    * EG: If a service descriptor is scoped, then the generated lazy service will also be scoped
* Keyed services are respected in the generation of lazy resolvers
    * EG: If a service descriptor has a key, then the generated lazy service will have that same key

### Usage

To automatically add lazy resolvers for all of your registered services, use an implementation of `ExtendedServiceProviderOptions` instead of `ServiceProviderOptions`.
Set the property `AddLazyResolvers` to `true` and pass this object to the `BuildServiceProvider` extension method.
