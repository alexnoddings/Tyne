using Microsoft.AspNetCore.Http;
using Tyne.AspNetCore.Routing;

namespace Microsoft.AspNetCore.Builder;

public static class RouteAuthorisationOptionsExtensions
{
    public static IRouteAuthorisationOptions AuthoriseRoute(this IRouteAuthorisationOptions routeAuthorisationOptions, Func<HttpContext, bool> authoriseWhen, params string[] policies)
    {
        ArgumentNullException.ThrowIfNull(routeAuthorisationOptions);
        return routeAuthorisationOptions.AuthoriseRoute(authoriseWhen, true, policies);
    }

    public static IRouteAuthorisationOptions AuthoriseRoute(this IRouteAuthorisationOptions routeAuthorisationOptions, PathString pathPrefix, params string[] policies)
    {
        ArgumentNullException.ThrowIfNull(routeAuthorisationOptions);
        return routeAuthorisationOptions.AuthoriseRoute(pathPrefix, true, policies);
    }

    public static IRouteAuthorisationOptions AuthoriseRoute(this IRouteAuthorisationOptions routeAuthorisationOptions, PathString pathPrefix, bool shouldHandleUnauthorised, params string[] policies)
    {
        ArgumentNullException.ThrowIfNull(routeAuthorisationOptions);

        bool AuthoriseWhenPathStartsWith(HttpContext httpContext) =>
            httpContext.Request.Path.StartsWithSegments(pathPrefix, StringComparison.OrdinalIgnoreCase);

        return routeAuthorisationOptions.AuthoriseRoute(AuthoriseWhenPathStartsWith, shouldHandleUnauthorised, policies);
    }

    public static IRouteAuthorisationOptions AuthoriseBlazorFrameworkRoutes(this IRouteAuthorisationOptions routeAuthorisationOptions, params string[] policies) =>
        routeAuthorisationOptions.AuthoriseBlazorFrameworkRoutes(PathString.Empty, true, policies);

    public static IRouteAuthorisationOptions AuthoriseBlazorFrameworkRoutes(this IRouteAuthorisationOptions routeAuthorisationOptions, bool shouldHandleUnauthorised, params string[] policies) =>
        routeAuthorisationOptions.AuthoriseBlazorFrameworkRoutes(PathString.Empty, shouldHandleUnauthorised, policies);

    public static IRouteAuthorisationOptions AuthoriseBlazorFrameworkRoutes(this IRouteAuthorisationOptions routeAuthorisationOptions, PathString pathPrefix, bool shouldHandleUnauthorised, params string[] policies)
    {
        ArgumentNullException.ThrowIfNull(routeAuthorisationOptions);

        bool AuthoriseWhen(HttpContext httpContext) =>
            httpContext.Request.Path.StartsWithSegments(pathPrefix, out var rest)
            && rest.StartsWithSegments("/_framework", StringComparison.OrdinalIgnoreCase);

        return routeAuthorisationOptions.AuthoriseRoute(AuthoriseWhen, shouldHandleUnauthorised, policies);
    }

    public static IRouteAuthorisationOptions AuthoriseBlazorContentRoutes(this IRouteAuthorisationOptions routeAuthorisationOptions, params string[] policies) =>
        routeAuthorisationOptions.AuthoriseBlazorContentRoutes(PathString.Empty, true, policies);

    public static IRouteAuthorisationOptions AuthoriseBlazorContentRoutes(this IRouteAuthorisationOptions routeAuthorisationOptions, bool shouldHandleUnauthorised, params string[] policies) =>
        routeAuthorisationOptions.AuthoriseBlazorContentRoutes(PathString.Empty, shouldHandleUnauthorised, policies);

    public static IRouteAuthorisationOptions AuthoriseBlazorContentRoutes(this IRouteAuthorisationOptions routeAuthorisationOptions, PathString pathPrefix, bool shouldHandleUnauthorised, params string[] policies)
    {
        ArgumentNullException.ThrowIfNull(routeAuthorisationOptions);

        bool AuthoriseWhen(HttpContext httpContext) =>
            httpContext.Request.Path.StartsWithSegments(pathPrefix, out var rest)
            && rest.StartsWithSegments("/_content", StringComparison.OrdinalIgnoreCase);

        return routeAuthorisationOptions.AuthoriseRoute(AuthoriseWhen, shouldHandleUnauthorised, policies);
    }
}