using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Microsoft.AspNetCore.Builder;

public static class NotFoundEndpointExtensions
{
	public static IEndpointConventionBuilder MapNotFoundFallback(this IEndpointRouteBuilder app) =>
		MapNotFoundFallback(app, "{*path:nonfile}");

    public static IEndpointConventionBuilder MapNotFoundFallback(this IEndpointRouteBuilder app, [StringSyntax("Route")] string pattern) =>
        MapNotFoundFallback(app, pattern, _ => { });

    public static IEndpointConventionBuilder MapNotFoundFallback(this IEndpointRouteBuilder app, [StringSyntax("Route")] string pattern, Action<HttpContext> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        Task AsyncHandler(HttpContext httpContext)
        {
            handler(httpContext);
            return Task.CompletedTask;
        }

        return MapNotFoundFallback(app, pattern, AsyncHandler);
    }

    public static IEndpointConventionBuilder MapNotFoundFallback(this IEndpointRouteBuilder app, [StringSyntax("Route")] string pattern, Func<HttpContext, Task> handler)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentException.ThrowIfNullOrEmpty(pattern);
        ArgumentNullException.ThrowIfNull(handler);

        return app.MapFallback(pattern, async httpContext =>
        {
            if (httpContext.Response.HasStarted)
                return;

            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            await handler(httpContext).ConfigureAwait(false);
        });
    }
}
