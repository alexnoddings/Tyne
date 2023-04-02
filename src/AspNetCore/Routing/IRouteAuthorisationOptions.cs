using Microsoft.AspNetCore.Http;

namespace Tyne.AspNetCore.Routing;

public interface IRouteAuthorisationOptions
{
    public IRouteAuthorisationOptions AuthoriseRoute(Func<HttpContext, bool> authoriseWhen, bool shouldHandleUnauthorised, params string[] policies);
}
