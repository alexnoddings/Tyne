using Microsoft.Extensions.Logging;

namespace Tyne.Blazor.Tables;

internal static partial class TyneTableLoggingExtensions
{
    [LoggerMessage(EventId = 101_001_301, Level = LogLevel.Debug, Message = "Creating filter context.")]
    public static partial void LogTableCreatingFilterContext(this ILogger logger);

    [LoggerMessage(EventId = 101_001_302, Level = LogLevel.Debug, Message = "Initialising table.")]
    public static partial void LogTableInitialising(this ILogger logger);

    [LoggerMessage(EventId = 101_001_303, Level = LogLevel.Debug, Message = "Reloading server data.")]
    public static partial void LogTableReloading(this ILogger logger);
}
