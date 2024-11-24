namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions for removing/replacing <see cref="ServiceDescriptor"/>s in <see cref="IServiceCollection"/>s.
/// </summary>
public static class TestingServiceCollectionModificationExtensions
{
    /// <summary>
    ///     Removes the service <typeparamref name="TService"/> from <paramref name="services"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service to remove. This is the <see cref="ServiceDescriptor.ServiceType"/>, not the <see cref="ServiceDescriptor.ImplementationType"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to remove the service from.</param>
    /// <returns>A reference to <paramref name="services"/> after the operation has completed to allow for chaining.</returns>
    public static IServiceCollection RemoveService<TService>(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        var serviceDescriptor = services.FirstOrDefault(serviceDescriptor => serviceDescriptor.ServiceType == typeof(TService));
        if (serviceDescriptor is not null)
            _ = services.Remove(serviceDescriptor);

        return services;
    }

    /// <summary>
    ///     Replaces the <typeparamref name="TService"/> service with <typeparamref name="TNewImplementation"/> with <paramref name="serviceLifetime"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service to remove. This is the <see cref="ServiceDescriptor.ServiceType"/>, not the <see cref="ServiceDescriptor.ImplementationType"/>.</typeparam>
    /// <typeparam name="TNewImplementation">The type to replace <typeparamref name="TService"/> with. This must implement <typeparamref name="TService"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to remove the service from.</param>
    /// <param name="serviceLifetime">The <see cref="ServiceLifetime"/> of the service.</param>
    /// <returns>A reference to <paramref name="services"/> after the operation has completed to allow for chaining.</returns>
    public static IServiceCollection ReplaceService<TService, TNewImplementation>(this IServiceCollection services, ServiceLifetime serviceLifetime) where TNewImplementation : TService
    {
        ArgumentNullException.ThrowIfNull(services);

        _ = services.RemoveService<TService>();
        services.Add(new ServiceDescriptor(typeof(TService), typeof(TNewImplementation), serviceLifetime));
        return services;
    }

    /// <summary>
    ///     Replaces the <typeparamref name="TService"/> service with the service created by <paramref name="implementationFactory"/> with <paramref name="serviceLifetime"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service to remove. This is the <see cref="ServiceDescriptor.ServiceType"/>, not the <see cref="ServiceDescriptor.ImplementationType"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to remove the service from.</param>
    /// <param name="implementationFactory">The factory that creates the replacement <typeparamref name="TService"/>.</param>
    /// <param name="serviceLifetime">The <see cref="ServiceLifetime"/> of the service.</param>
    /// <returns>A reference to <paramref name="services"/> after the operation has completed to allow for chaining.</returns>
    public static IServiceCollection ReplaceService<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory, ServiceLifetime serviceLifetime) where TService : class
    {
        ArgumentNullException.ThrowIfNull(services);

        _ = services.RemoveService<TService>();
        services.Add(new ServiceDescriptor(typeof(TService), implementationFactory, serviceLifetime));
        return services;
    }

    /// <summary>
    ///     Replaces the <typeparamref name="TService"/> service with <paramref name="implementationInstance"/>. This has a <see cref="ServiceLifetime.Singleton"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service to remove. This is the <see cref="ServiceDescriptor.ServiceType"/>, not the <see cref="ServiceDescriptor.ImplementationType"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to remove the service from.</param>
    /// <param name="implementationInstance">The instance of <typeparamref name="TService"/>.</param>
    /// <returns>A reference to <paramref name="services"/> after the operation has completed to allow for chaining.</returns>
    public static IServiceCollection ReplaceService<TService>(this IServiceCollection services, TService implementationInstance) where TService : class
    {
        ArgumentNullException.ThrowIfNull(services);

        _ = services.RemoveService<TService>();
        services.Add(new ServiceDescriptor(typeof(TService), implementationInstance));
        return services;
    }

    /// <summary>
    ///     Replaces the <typeparamref name="TService"/> service with <typeparamref name="TNewImplementation"/> with <see cref="ServiceLifetime.Transient"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service to remove. This is the <see cref="ServiceDescriptor.ServiceType"/>, not the <see cref="ServiceDescriptor.ImplementationType"/>.</typeparam>
    /// <typeparam name="TNewImplementation">The type to replace <typeparamref name="TService"/> with. This must implement <typeparamref name="TService"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to remove the service from.</param>
    /// <returns>A reference to <paramref name="services"/> after the operation has completed to allow for chaining.</returns>
    public static IServiceCollection ReplaceServiceTransient<TService, TNewImplementation>(this IServiceCollection services) where TNewImplementation : TService =>
        services.ReplaceService<TService, TNewImplementation>(ServiceLifetime.Transient);

    /// <summary>
    ///     Replaces the <typeparamref name="TService"/> service with the service created by <paramref name="implementationFactory"/> with <see cref="ServiceLifetime.Transient"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service to remove. This is the <see cref="ServiceDescriptor.ServiceType"/>, not the <see cref="ServiceDescriptor.ImplementationType"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to remove the service from.</param>
    /// <param name="implementationFactory">The factory that creates the replacement <typeparamref name="TService"/>.</param>
    /// <returns>A reference to <paramref name="services"/> after the operation has completed to allow for chaining.</returns>
    public static IServiceCollection ReplaceServiceTransient<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory) where TService : class =>
        services.ReplaceService(implementationFactory, ServiceLifetime.Transient);

    /// <summary>
    ///     Replaces the <typeparamref name="TService"/> service with <typeparamref name="TNewImplementation"/> with <see cref="ServiceLifetime.Scoped"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service to remove. This is the <see cref="ServiceDescriptor.ServiceType"/>, not the <see cref="ServiceDescriptor.ImplementationType"/>.</typeparam>
    /// <typeparam name="TNewImplementation">The type to replace <typeparamref name="TService"/> with. This must implement <typeparamref name="TService"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to remove the service from.</param>
    /// <returns>A reference to <paramref name="services"/> after the operation has completed to allow for chaining.</returns>
    public static IServiceCollection ReplaceServiceScoped<TService, TNewImplementation>(this IServiceCollection services) where TNewImplementation : TService =>
        services.ReplaceService<TService, TNewImplementation>(ServiceLifetime.Scoped);

    /// <summary>
    ///     Replaces the <typeparamref name="TService"/> service with the service created by <paramref name="implementationFactory"/> with <see cref="ServiceLifetime.Scoped"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service to remove. This is the <see cref="ServiceDescriptor.ServiceType"/>, not the <see cref="ServiceDescriptor.ImplementationType"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to remove the service from.</param>
    /// <param name="implementationFactory">The factory that creates the replacement <typeparamref name="TService"/>.</param>
    /// <returns>A reference to <paramref name="services"/> after the operation has completed to allow for chaining.</returns>
    public static IServiceCollection ReplaceServiceScoped<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory) where TService : class =>
        services.ReplaceService(implementationFactory, ServiceLifetime.Scoped);

    /// <summary>
    ///     Replaces the <typeparamref name="TService"/> service with <typeparamref name="TNewImplementation"/> with <see cref="ServiceLifetime.Singleton"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service to remove. This is the <see cref="ServiceDescriptor.ServiceType"/>, not the <see cref="ServiceDescriptor.ImplementationType"/>.</typeparam>
    /// <typeparam name="TNewImplementation">The type to replace <typeparamref name="TService"/> with. This must implement <typeparamref name="TService"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to remove the service from.</param>
    /// <returns>A reference to <paramref name="services"/> after the operation has completed to allow for chaining.</returns>
    public static IServiceCollection ReplaceServiceSingleton<TService, TNewImplementation>(this IServiceCollection services) where TNewImplementation : TService =>
        services.ReplaceService<TService, TNewImplementation>(ServiceLifetime.Singleton);

    /// <summary>
    ///     Replaces the <typeparamref name="TService"/> service with the service created by <paramref name="implementationFactory"/> with <see cref="ServiceLifetime.Singleton"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service to remove. This is the <see cref="ServiceDescriptor.ServiceType"/>, not the <see cref="ServiceDescriptor.ImplementationType"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to remove the service from.</param>
    /// <param name="implementationFactory">The factory that creates the replacement <typeparamref name="TService"/>.</param>
    /// <returns>A reference to <paramref name="services"/> after the operation has completed to allow for chaining.</returns>
    public static IServiceCollection ReplaceServiceSingleton<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory) where TService : class =>
        services.ReplaceService(implementationFactory, ServiceLifetime.Singleton);
}
