using Microsoft.Extensions.Logging;

namespace Tyne.HttpMediator.Server;

internal static partial class ExceptionHandlerMiddlewareLogging
{
    [LoggerMessage(EventId = 101_003_002, Level = LogLevel.Error, Message = "Unhandled exception in the HTTP mediator pipeline.")]
    public static partial void LogUnhandledHttpMediatorPipelineException(this ILogger logger, Exception exception);
}
