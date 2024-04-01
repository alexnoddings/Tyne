using Microsoft.Extensions.Logging;

namespace Tyne.HttpMediator.Client;

internal static partial class HttpMediatorLogging
{
    [LoggerMessage(EventId = 101_002_120, Level = LogLevel.Warning, Message = "Tried to send null request, returning bad request.")]
    public static partial void LogNullRequest(this ILogger logger);
}
