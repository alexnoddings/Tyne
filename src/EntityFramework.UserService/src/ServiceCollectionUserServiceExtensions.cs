using Microsoft.AspNetCore.Http;
using Tyne;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionUserServiceExtensions
{
    public static TyneBuilder AddUserServiceFromClaim(this TyneBuilder builder, string claimKey)
    {
        ArgumentException.ThrowIfNullOrEmpty(claimKey);

        Guid? GetUserIdFromHttpContext(IServiceProvider services)
        {
            Guid? userId = null;
            var httpContext = services.GetService<IHttpContextAccessor>()?.HttpContext;
            if (httpContext is not null)
            {
                var claimValue = httpContext.User.Claims.FirstOrDefault(claim => claim.Type == claimKey)?.Value;
                if (claimValue is not null && Guid.TryParse(claimValue, out var claimUserId))
                    userId = claimUserId;
            }
            return userId;
        }

        return builder.AddUserService(GetUserIdFromHttpContext);
    }
}
