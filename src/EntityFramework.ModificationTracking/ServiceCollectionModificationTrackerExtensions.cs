using Microsoft.Extensions.DependencyInjection;
using Tyne.EntityFramework;

namespace Tyne;

public static class ServiceCollectionModificationTrackerExtensions
{
    public static TyneBuilder AddDbContextModificationTracker(this TyneBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        if (!builder.Services.Any(serviceDescriptor => serviceDescriptor.ServiceType == typeof(ITyneUserService)))
            throw new InvalidOperationException($"Please ensure an {nameof(ITyneUserService)} is registered before calling {nameof(AddDbContextModificationTracker)}.");

        builder.Services.AddScoped<IDbContextModificationTracker, DbContextModificationTracker>();

        return builder;
    }
}
