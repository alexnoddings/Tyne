using Microsoft.Extensions.Logging;

namespace Tyne.Blazor;

internal static partial class TyneFormBaseLoggingExtensions
{
    [LoggerMessage(EventId = 101_001_201, Level = LogLevel.Error, Message = "Error while initialising form.")]
    public static partial void LogInitialiseFormError(this ILogger logger, Exception exception);

    [LoggerMessage(EventId = 101_001_202, Level = LogLevel.Error, Message = "Error while opening form.")]
    public static partial void LogOpenFormError(this ILogger logger, Exception exception);

    [LoggerMessage(EventId = 101_001_203, Level = LogLevel.Error, Message = "Error while saving form.")]
    public static partial void LogSaveFormError(this ILogger logger, Exception exception);

    [LoggerMessage(EventId = 101_001_204, Level = LogLevel.Error, Message = "Error while invoking on form saved.")]
    public static partial void LogOnSavedFormError(this ILogger logger, Exception exception);

    [LoggerMessage(EventId = 101_001_205, Level = LogLevel.Error, Message = "Error while closing form.")]
    public static partial void LogCloseFormError(this ILogger logger, Exception exception);
}
