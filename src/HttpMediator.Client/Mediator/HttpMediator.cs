using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Tyne.HttpMediator.Client;

internal class HttpMediator : IHttpMediator
{
    private readonly ILogger _logger;
    private readonly HttpMediatorMiddlewarePipeline _middlewarePipeline;

    public HttpMediator(ILogger<HttpMediator> logger, HttpMediatorMiddlewarePipeline middlewarePipeline)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _middlewarePipeline = middlewarePipeline ?? throw new ArgumentNullException(nameof(middlewarePipeline));
    }

    public async Task<HttpResult<TResponse>> SendAsync<TResponse>(IHttpRequestBase<TResponse> request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            _logger.LogNullRequest();
            return HttpResult.Codes.BadRequest<TResponse>("Request was null.");
        }

        using var loggerRequestScope = _logger.BeginRequestScope(request);

        var genericMethodInfo = ExecuteAsyncMethodInfo.MakeGenericMethod(request.GetType(), typeof(TResponse));
        HttpResult<TResponse> result;
        try
        {
            var task = (Task<HttpResult<TResponse>>)genericMethodInfo.Invoke(this, [request])!;
            result = await task.ConfigureAwait(false);
        }
        catch (TargetInvocationException invocationException)
        {
            // Reflection wraps exceptions in a TargetInvocationException,
            // so unwrap the base exception since that's what we care about
            throw invocationException.GetBaseException();
        }

        return result;
    }

    [SuppressMessage("Major Code Smell", "S3011: Reflection should not be used to increase accessibility of classes, methods, or fields.", Justification = "We are reflecting on a method private to this class.")]
    private static readonly MethodInfo ExecuteAsyncMethodInfo =
        typeof(HttpMediator)
        .GetMethod(nameof(ExecuteAsync), BindingFlags.Instance | BindingFlags.NonPublic)
        ?? throw new InvalidOperationException($"No \"{nameof(ExecuteAsync)}\" method found on \"{nameof(HttpMediator)}\".");

    private Task<HttpResult<TResponse>> ExecuteAsync<TRequest, TResponse>(TRequest request) where TRequest : IHttpRequestBase<TResponse> =>
        _middlewarePipeline.ExecuteAsync<TRequest, TResponse>(request);
}
