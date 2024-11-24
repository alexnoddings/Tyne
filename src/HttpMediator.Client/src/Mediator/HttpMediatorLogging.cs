using Microsoft.Extensions.Logging;

namespace Tyne.HttpMediator.Client;

internal static partial class HttpMediatorLogging
{
    private static readonly Func<ILogger, Type, IDisposable?> _requestScope =
        LoggerMessage.DefineScope<Type>("Processing request {RequestType}");

    public static IDisposable? BeginRequestScope<TResponse>(this ILogger logger, IHttpRequestBase<TResponse> request) =>
        _requestScope(logger, request.GetType());

    [LoggerMessage(EventId = 101_002_120, Level = LogLevel.Warning, Message = "Tried to send null request, returning bad request.")]
    public static partial void LogNullRequest(this ILogger logger);
}
