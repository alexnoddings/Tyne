using Microsoft.Extensions.Logging;

namespace Tyne.HttpMediator.Client;

internal static partial class HttpSenderMiddlewareLogging
{
    [LoggerMessage(EventId = 101_002_110, Level = LogLevel.Debug, Message = "Sending HTTP request to '{ApiUri}' with request {Request}.")]
    public static partial void LogSendingHttpRequest(this ILogger logger, string? apiUri, object request);
}
