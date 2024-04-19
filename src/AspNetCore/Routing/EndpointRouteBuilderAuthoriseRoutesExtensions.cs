using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Tyne.AspNetCore.Routing;

namespace Microsoft.AspNetCore.Builder;

public static class EndpointRouteBuilderAuthoriseRoutesExtensions
{
    public static IApplicationBuilder UseTyneRouteAuthorisation(this IApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var options = builder.ApplicationServices.GetRequiredService<IOptions<RouteAuthorisationOptions>>().Value;

        foreach (var routeAuthorisation in options.RouteAuthorisations)
        {
            var middleware =
                routeAuthorisation.Policies.Length == 0
                ? CreateRouteAuthorisationMiddlewareWithoutPolicies(routeAuthorisation.ShouldHandleUnauthorised)
                : CreateRouteAuthorisationMiddlewareWithPolicies(routeAuthorisation.ShouldHandleUnauthorised, routeAuthorisation.Policies);

            builder.UseWhen(routeAuthorisation.AuthoriseWhen, subBuilder => subBuilder.Use(middleware));
        }

        return builder;
    }

    private static Task<AuthorizationPolicy> CreateDefaultAuthorisationPolicyAsync(IAuthorizationPolicyProvider policyProvider) =>
        policyProvider.GetDefaultPolicyAsync();

    private static Func<HttpContext, RequestDelegate, Task> CreateRouteAuthorisationMiddlewareWithoutPolicies(bool shouldHandleUnauthorised) =>
        CreateRouteAuthorisationMiddleware(shouldHandleUnauthorised, CreateDefaultAuthorisationPolicyAsync);

    private static Func<HttpContext, RequestDelegate, Task> CreateRouteAuthorisationMiddlewareWithPolicies(bool shouldHandleUnauthorised, string[] policyNames)
    {
        async Task<AuthorizationPolicy> CreateAuthorisationPolicy(IAuthorizationPolicyProvider policyProvider)
        {
            var policies = new List<AuthorizationPolicy>();
            foreach (var policyName in policyNames)
            {
                var singlePolicy = await policyProvider.GetPolicyAsync(policyName).ConfigureAwait(false);
                if (singlePolicy is not null)
                    policies.Add(singlePolicy);
            }

#if NET8_0_OR_GREATER
            var policy = await AuthorizationPolicy.CombineAsync(policyProvider, Enumerable.Empty<IAuthorizeData>(), policies).ConfigureAwait(false);
#else
            var policy = await AuthorizationPolicy.CombineAsync(policyProvider, [], policies).ConfigureAwait(false);
#endif
            return policy ?? await policyProvider.GetDefaultPolicyAsync().ConfigureAwait(false);
        }

        return CreateRouteAuthorisationMiddleware(shouldHandleUnauthorised, CreateAuthorisationPolicy);
    }

    private static Func<HttpContext, RequestDelegate, Task> CreateRouteAuthorisationMiddleware(bool shouldHandleUnauthorised, Func<IAuthorizationPolicyProvider, Task<AuthorizationPolicy>> authorisationPolicyFactory)
    {
        async Task RunAsync(HttpContext httpContext, RequestDelegate next)
        {
            var policyProvider = httpContext.RequestServices.GetRequiredService<IAuthorizationPolicyProvider>();
            var policyEvaluator = httpContext.RequestServices.GetRequiredService<IPolicyEvaluator>();

            var policy = await authorisationPolicyFactory(policyProvider).ConfigureAwait(false);

            var authenticateResult = await policyEvaluator.AuthenticateAsync(policy, httpContext).ConfigureAwait(false);
            var authoriseResult = await policyEvaluator.AuthorizeAsync(policy, authenticateResult, httpContext, httpContext).ConfigureAwait(false);

            if (authoriseResult.Succeeded)
            {
                await next(httpContext).ConfigureAwait(false);
            }
            else
            {
                if (shouldHandleUnauthorised)
                {
                    var authorizationMiddlewareResultHandler = httpContext.RequestServices.GetRequiredService<IAuthorizationMiddlewareResultHandler>();
                    await authorizationMiddlewareResultHandler.HandleAsync(next, httpContext, policy, authoriseResult).ConfigureAwait(false);
                }
                else
                {
                    if (authenticateResult.Succeeded)
                        await httpContext.ForbidAsync().ConfigureAwait(false);
                    else
                        await httpContext.ChallengeAsync().ConfigureAwait(false);
                }
            }
        }

        return RunAsync;
    }
}
