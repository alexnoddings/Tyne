using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Tyne;
using Tyne.HttpMediator;

namespace Tyne.HttpMediator.Client;

/// <summary>
///     Handles executing the <see cref="HttpMediatorMiddlewareCollection"/>.
/// </summary>
internal sealed class HttpMediatorMiddlewarePipeline
{
    private readonly IServiceProvider _services;
    private readonly HttpMediatorMiddlewareCollection _middlewareCollection;

    public HttpMediatorMiddlewarePipeline(IServiceProvider services, HttpMediatorMiddlewareCollection middlewareCollection)
    {
        _services = services ?? throw new ArgumentNullException(nameof(services));
        _middlewareCollection = middlewareCollection ?? throw new ArgumentNullException(nameof(middlewareCollection));
    }

    public Task<HttpResult<TResponse>> ExecuteAsync<TRequest, TResponse>(TRequest request) where TRequest : IHttpRequestBase<TResponse>
    {
        var pipeline = BuildPipeline<TRequest, TResponse>();
        return pipeline.Invoke(request);
    }

    private HttpMediatorDelegate<TRequest, TResponse> BuildPipeline<TRequest, TResponse>() where TRequest : IHttpRequestBase<TResponse>
    {
        var current = FallthroughDelegateCache<TRequest, TResponse>.Instance;
        var middlewareInstances = _middlewareCollection
            .Select(_services.GetRequiredService)
            .OfType<IHttpMediatorMiddleware>()
            .Reverse()
            .ToList();

        foreach (var middlewareInstance in middlewareInstances)
        {
            var next = current;
            current = ExecuteMiddleware;

            [StackTraceHidden]
            Task<HttpResult<TResponse>> ExecuteMiddleware(TRequest request) =>
                middlewareInstance.InvokeAsync(request, next);
        }

        return current;
    }

    // Statically caches HttpMediatorDelegates for the final, fallthrough delegate in the pipeline.
    private static class FallthroughDelegateCache<TRequest, TResponse> where TRequest : IHttpRequestBase<TResponse>
    {
        // This delegate shouldn't be called as a middleware should terminate the pipeline before this is reached
        internal static readonly HttpMediatorDelegate<TRequest, TResponse> Instance =
            _ => throw new InvalidOperationException("HTTP Mediator pipeline did not terminate gracefully.");
    }
}
