using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Tyne;
using Tyne.HttpMediator;

namespace Tyne.HttpMediator.Server;

/// <summary>
///     Handles executing the <see cref="HttpMediatorMiddlewareCollection"/>.
/// </summary>
[SuppressMessage("Microsoft.Performance", "CA1812: Avoid uninstantiated internal classes.", Justification = "Instantiated by DI.")]
internal sealed class HttpMediatorMiddlewarePipeline
{
    private readonly HttpMediatorMiddlewareCollection _middlewareCollection;

    public HttpMediatorMiddlewarePipeline(HttpMediatorMiddlewareCollection middlewareCollection)
    {
        _middlewareCollection = middlewareCollection ?? throw new ArgumentNullException(nameof(middlewareCollection));
    }

    public Task<HttpResult<TResponse>> ExecuteAsync<TRequest, TResponse>(HttpContext httpContext, TRequest request) where TRequest : IHttpRequestBase<TResponse>
    {
        var pipeline = BuildPipeline<TRequest, TResponse>(httpContext.RequestServices);
        return pipeline.Invoke(httpContext, request);
    }

    [SuppressMessage("Major Code Smell", "S1172: Unused method parameters should be removed.", Justification = "False positive.")]
    private HttpMediatorDelegate<TRequest, TResponse> BuildPipeline<TRequest, TResponse>(IServiceProvider services) where TRequest : IHttpRequestBase<TResponse>
    {
        var current = FallthroughDelegateCache<TRequest, TResponse>.Instance;
        var middlewareInstances = _middlewareCollection
            .Select(services.GetRequiredService)
            .OfType<IHttpMediatorMiddleware>()
            .Reverse()
            .ToList();

        foreach (var middlewareInstance in middlewareInstances)
        {
            var next = current;
            current = ExecuteMiddleware;

            [StackTraceHidden]
            Task<HttpResult<TResponse>> ExecuteMiddleware(HttpContext httpContext, TRequest request) =>
                middlewareInstance.InvokeAsync(httpContext, request, next);
        }

        return current;
    }

    // Statically caches HttpMediatorDelegates for the final, fallthrough delegate in the pipeline.
    private static class FallthroughDelegateCache<TRequest, TResponse> where TRequest : IHttpRequestBase<TResponse>
    {
        // This delegate shouldn't be called as a middleware should terminate the pipeline before this is reached
        internal static readonly HttpMediatorDelegate<TRequest, TResponse> Instance =
            (_, _) => throw new InvalidOperationException("HTTP Mediator pipeline did not terminate gracefully.");
    }
}
