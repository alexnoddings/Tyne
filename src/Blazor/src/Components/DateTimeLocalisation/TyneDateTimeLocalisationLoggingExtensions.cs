using Microsoft.Extensions.Logging;

namespace Tyne.Blazor.Localisation;

internal static partial class TyneDateTimeLocalisationLoggingExtensions
{
    [LoggerMessage(EventId = 101_001_001, Level = LogLevel.Error, Message = "TimeZone '{TimeZoneId}' reported by browser was not found in system cache. Falling back to custom time zone.")]
    public static partial void LogCouldNotLoadUserTimeZone(this ILogger logger, string timeZoneId);
}
