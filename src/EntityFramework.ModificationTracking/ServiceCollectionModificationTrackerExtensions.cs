using Tyne;
using Tyne.EntityFramework;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionModificationTrackerExtensions
{
    public static TyneBuilder AddDbContextModificationTracker(this TyneBuilder builder) =>
        AddDbContextModificationTracker(builder, ServiceLifetime.Scoped);

    public static TyneBuilder AddDbContextModificationTracker(this TyneBuilder builder, ServiceLifetime serviceLifetime)
    {
        ArgumentNullException.ThrowIfNull(builder);

        if (!builder.Services.Any(serviceDescriptor => serviceDescriptor.ServiceType == typeof(ITyneUserService)))
            throw new InvalidOperationException($"Please ensure an {nameof(ITyneUserService)} is registered before calling {nameof(AddDbContextModificationTracker)}.");

        builder.Services.Add(new(typeof(IDbContextModificationTracker), typeof(DbContextModificationTracker), serviceLifetime));

        return builder;
    }
}
