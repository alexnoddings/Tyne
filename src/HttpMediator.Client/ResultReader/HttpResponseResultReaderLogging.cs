using Microsoft.Extensions.Logging;
using Tyne.Internal.HttpMediator;

namespace Tyne.HttpMediator.Client;

internal static partial class HttpResponseResultReaderLogging
{
    [LoggerMessage(EventId = 101_002_001, Level = LogLevel.Error, Message = "Input does not contain any JSON tokens.")]
    public static partial void LogJsonValueNoInput(this ILogger logger, Exception exception);

    [LoggerMessage(EventId = 101_002_002, Level = LogLevel.Error, Message = "JSON value could not be converted.")]
    public static partial void LogJsonValueCouldNotBeConverted(this ILogger logger, Exception exception);

    [LoggerMessage(EventId = 101_002_003, Level = LogLevel.Error, Message = "Unknown JSON read error.")]
    public static partial void LogJsonValueUnknownError(this ILogger logger, Exception exception);

    [LoggerMessage(EventId = 101_002_004, Level = LogLevel.Error, Message = "JSON read returned a null value.")]
    public static partial void LogJsonValueNull(this ILogger logger);

    [LoggerMessage(EventId = 101_002_010, Level = LogLevel.Error, Message = $"JSON read returned a null {nameof(ProblemDetails)}.")]
    public static partial void LogJsonProblemDetailsNull(this ILogger logger);

}
