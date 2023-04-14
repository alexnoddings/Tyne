using Microsoft.AspNetCore.Http;

namespace Tyne.AspNetCore.Routing;

internal sealed class RouteAuthorisationOptions : IRouteAuthorisationOptions
{
    internal List<RouteAuthorisation> RouteAuthorisations { get; } = new();

    public IRouteAuthorisationOptions AuthoriseRoute(Func<HttpContext, bool> authoriseWhen, bool shouldHandleUnauthorised, params string[] policies)
    {
        ArgumentNullException.ThrowIfNull(authoriseWhen);
        ArgumentNullException.ThrowIfNull(policies);

        if (policies.Any(string.IsNullOrEmpty))
            throw new ArgumentException("Policy names cannot be null or empty.", nameof(policies));

        RouteAuthorisations.Add(new(authoriseWhen, shouldHandleUnauthorised, policies));

        return this;
    }
}
