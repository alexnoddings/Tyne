using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Microsoft.AspNetCore.Builder;

[SuppressMessage("Info Code Smell", "S1133: Deprecated code should be removed.", Justification = "Methods are deprecated to allow for a graceful transition. They will be removed in a future version.")]
public static class NotFoundEndpointExtensions
{
    private const string ObsoleteDiagnosticMessage = $"Use {nameof(MapFallbackToNotFound)} instead.";
    private const string ObsoleteDiagnosticId = "TYNEOLD";

    [Obsolete(message: ObsoleteDiagnosticMessage, DiagnosticId = ObsoleteDiagnosticId)]
    public static IEndpointConventionBuilder MapNotFoundFallback(this IEndpointRouteBuilder app) =>
        MapFallbackToNotFound(app);

    [Obsolete(message: ObsoleteDiagnosticMessage, DiagnosticId = ObsoleteDiagnosticId)]
    public static IEndpointConventionBuilder MapNotFoundFallback(this IEndpointRouteBuilder app, [StringSyntax("Route")] string pattern) =>
        MapFallbackToNotFound(app, pattern);

    [Obsolete(message: ObsoleteDiagnosticMessage, DiagnosticId = ObsoleteDiagnosticId)]
    public static IEndpointConventionBuilder MapNotFoundFallback(this IEndpointRouteBuilder app, [StringSyntax("Route")] string pattern, Action<HttpContext> handler) =>
        MapFallbackToNotFound(app, pattern, handler);

    [Obsolete(message: ObsoleteDiagnosticMessage, DiagnosticId = ObsoleteDiagnosticId)]
    public static IEndpointConventionBuilder MapNotFoundFallback(this IEndpointRouteBuilder app, [StringSyntax("Route")] string pattern, Func<HttpContext, Task> handler) =>
        MapFallbackToNotFound(app, pattern, handler);

    public static IEndpointConventionBuilder MapFallbackToNotFound(this IEndpointRouteBuilder app) =>
        MapFallbackToNotFound(app, "{*path:nonfile}");

    public static IEndpointConventionBuilder MapFallbackToNotFound(this IEndpointRouteBuilder app, [StringSyntax("Route")] string pattern) =>
        MapFallbackToNotFound(app, pattern, _ => { });

    public static IEndpointConventionBuilder MapFallbackToNotFound(this IEndpointRouteBuilder app, [StringSyntax("Route")] string pattern, Action<HttpContext> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        Task AsyncHandler(HttpContext httpContext)
        {
            handler(httpContext);
            return Task.CompletedTask;
        }

        return MapFallbackToNotFound(app, pattern, AsyncHandler);
    }

    public static IEndpointConventionBuilder MapFallbackToNotFound(this IEndpointRouteBuilder app, [StringSyntax("Route")] string pattern, Func<HttpContext, Task> handler)
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
