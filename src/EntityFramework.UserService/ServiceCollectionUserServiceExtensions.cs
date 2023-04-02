using Microsoft.AspNetCore.Http;
using Tyne;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionUserServiceExtensions
{
    private sealed class SimpleUserService : ITyneUserService
    {
        public required Guid? UserId { get; init; }
    }

    public static TyneBuilder AddUserService(this TyneBuilder builder, Func<IServiceProvider, Guid?> getUserId, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(getUserId);

        ITyneUserService CreateDefaultUserService(IServiceProvider services)
        {
            var userId = getUserId(services);
            return new SimpleUserService { UserId = userId };
        }

        builder.Services.Add(new ServiceDescriptor(typeof(ITyneUserService), CreateDefaultUserService, lifetime));

        return builder;
    }

    public static TyneBuilder AddUserService(this TyneBuilder builder, Func<IServiceProvider, ITyneUserService> factory, ServiceLifetime lifetime = ServiceLifetime.Scoped)
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

    public static TyneBuilder AddUserServiceFromClaim(this TyneBuilder builder, string claimKey)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentException.ThrowIfNullOrEmpty(claimKey);

        ITyneUserService CreateDefaultUserService(IServiceProvider services)
        {
            Guid? userId = null;
            var httpContext = services.GetService<IHttpContextAccessor>()?.HttpContext;
            if (httpContext is not null)
            {
                var claimValue = httpContext.User.Claims.FirstOrDefault(claim => claim.Type == claimKey)?.Value;
                if (claimValue is not null && Guid.TryParse(claimValue, out var claimUserId))
                    userId = claimUserId;
            }

            return new SimpleUserService { UserId = userId };
        }

        builder.Services.Add(new ServiceDescriptor(typeof(ITyneUserService), CreateDefaultUserService, ServiceLifetime.Scoped));

        return builder;
    }
}
