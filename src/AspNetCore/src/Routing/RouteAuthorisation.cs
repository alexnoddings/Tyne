using Microsoft.AspNetCore.Http;

namespace Tyne.AspNetCore.Routing;

internal sealed record RouteAuthorisation(Func<HttpContext, bool> AuthoriseWhen, bool ShouldHandleUnauthorised, string[] Policies);
