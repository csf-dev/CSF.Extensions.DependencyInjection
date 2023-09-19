namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// A specialisation of <see cref="ServiceProviderOptions"/> which is used to add functionality from
    /// CSF.Extensions.DependencyInjection to the service provider.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When an instance of this type is used, with the <see cref="ServiceCollectionExtensions.BuildServiceProvider(ServiceCollection,ExtendedServiceProviderOptions)"/>
    /// extension method, it activates additional customisation options for the created service provider.
    /// </para>
    /// <para>
    /// In this case, the originally-supported options from <see cref="ServiceProviderOptions"/> will still function as well.
    /// Using this class of extended options does not preclude using built-in Microsoft-specified options.
    /// </para>
    /// </remarks>
    /// <seealso cref="CSF.Extensions.DependencyInjection.Lazy.LazyServicesExtender"/>
    public class ExtendedServiceProviderOptions : ServiceProviderOptions
    {
        /// <summary>
        /// Gets or sets a value which determines whether or not lazy resolvers will be added to the service provider.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When this option is set to <see langword="true" /> then every service registered within the service collection will
        /// be made available from the service provider as a <see cref="System.Lazy{T}"/> of the same service type.
        /// When this option is set to <see langword="false" /> then no modification is made to the service collection for the purpose
        /// of resolving lazy services.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>
        /// If the service collection had <c>MyServiceClass</c> registered as the interface <c>IMyService</c> and this option were
        /// <see langword="true" /> then it would be possible to resolve a <c>Lazy&lt;IMyService&gt;</c> and it would correctly
        /// resolve as a lazy instance of <c>MyServiceClass</c>.
        /// </para>
        /// </example>
        public bool AddLazyResolvers { get; set; }
    }
}