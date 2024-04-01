using Microsoft.Extensions.Logging;

namespace Tyne.HttpMediator.Server;

internal static partial class HttpRequestHandlerLogging
{
    [LoggerMessage(EventId = 101_003_003, Level = LogLevel.Debug, Message = "Request was null, returning bad request.")]
    public static partial void LogNullRequest(this ILogger logger);


    [LoggerMessage(EventId = 101_003_004, Level = LogLevel.Debug, Message = "Handling request {RequestType}.")]
    private static partial void LogHandlingRequest(this ILogger logger, Type requestType);

    public static void LogHandlingRequest<TRequest>(this ILogger logger) =>
        LogHandlingRequest(logger, typeof(TRequest));
}
