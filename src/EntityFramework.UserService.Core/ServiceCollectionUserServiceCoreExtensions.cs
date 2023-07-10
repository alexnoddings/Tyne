using Tyne;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionUserServiceCoreExtensions
{
    public static TyneBuilder AddUserService(this TyneBuilder builder, Func<IServiceProvider, Guid?> getUserId, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(getUserId);

        ITyneUserService CreateDeferredUserService(IServiceProvider services) =>
            new DeferredUserService(services, getUserId);

        builder.Services.Add(new ServiceDescriptor(typeof(ITyneUserService), CreateDeferredUserService, lifetime));

        return builder;
    }

    public static TyneBuilder AddUserService<TUserService>(this TyneBuilder builder, Func<IServiceProvider, TUserService> factory, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TUserService : class, ITyneUserService
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(factory);

        builder.Services.Add(new ServiceDescriptor(typeof(ITyneUserService), factory, lifetime));

        return builder;
    }

    public static TyneBuilder AddUserService<TService>(this TyneBuilder builder, ServiceLifetime lifetime = ServiceLifetime.Scoped) where TService : ITyneUserService
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.Add(new ServiceDescriptor(typeof(ITyneUserService), typeof(TService), lifetime));

        return builder;
    }
}
