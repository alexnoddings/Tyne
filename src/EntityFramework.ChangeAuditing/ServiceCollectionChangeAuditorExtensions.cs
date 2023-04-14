using Microsoft.Extensions.DependencyInjection;
using Tyne.EntityFramework;

namespace Tyne;

public static class ServiceCollectionChangeAuditorExtensions
{
    public static TyneBuilder AddDbContextChangeAuditor(this TyneBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        if (!builder.Services.Any(serviceDescriptor => serviceDescriptor.ServiceType == typeof(ITyneUserService)))
            throw new InvalidOperationException($"Please ensure an {nameof(ITyneUserService)} is registered before calling {nameof(AddDbContextChangeAuditor)}.");

        builder.Services.AddScoped<IDbContextChangeAuditor, DbContextChangeAuditor>();

        return builder;
    }
}
