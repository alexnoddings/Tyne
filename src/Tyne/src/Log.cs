using Microsoft.Extensions.Logging;

namespace Tyne;

internal static partial class Log
{
	[LoggerMessage(101_001_001, LogLevel.Error, "An error occurred while executing the action.", EventName = nameof(ExceptionRunningAction))]
	public static partial void ExceptionRunningAction(this ILogger logger, Exception exception);
}
