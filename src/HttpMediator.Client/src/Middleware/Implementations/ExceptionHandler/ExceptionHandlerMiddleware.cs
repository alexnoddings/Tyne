using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Tyne.HttpMediator.Client;

/// <summary>
///     Middleware which handles exceptions in the pipeline.
/// </summary>
internal sealed class ExceptionHandlerMiddleware : IHttpMediatorMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger)
    {
        _logger = logger;
    }

    [SuppressMessage("Design", "CA1031: Do not catch general exception types.", Justification = "Root exception handler for pipeline.")]
    public async Task<HttpResult<TResponse>> InvokeAsync<TRequest, TResponse>(TRequest request, HttpMediatorDelegate<TRequest, TResponse> next) where TRequest : IHttpRequestBase<TResponse>
    {
        try
        {
            return await next(request).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            _logger.LogUnhandledHttpMediatorPipelineException(exception);
            var error = Error.From(ErrorsCodes.UnhandledExceptionMiddleware, Error.Default.Message, exception);
            // We don't know what happened, fall back to it being a bad request
            return HttpResult.Codes.BadRequest<TResponse>(error);
        }
    }
}
