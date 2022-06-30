using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace Tyne.EntityFramework;

/// <summary>
///     Intercepts <see cref="DbCommand"/>s prior to execution to provide an artificial delay.
/// </summary>
/// <remarks>
///     See <see cref="Microsoft.EntityFrameworkCore.DbSlowByExtensions.EnableDebugSlowBy(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder, bool)"/> for more info.
/// </remarks>
public class DbSlowByCommandInterceptor : DbCommandInterceptor
{
	// Used to search for the SLOW BY tag at the head of the command, and capture the actual amount to slow by
	private static Regex SlowByRegex { get; } = new Regex("-- SLOW BY ([0-9]+)ms", RegexOptions.Compiled | RegexOptions.IgnoreCase);

	public override async ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
	{
		// Search for the SLOW BY tag
		Match slowByMatch = SlowByRegex.Match(command.CommandText);
		// If one is found, and there are as least 2 groups (the first is the full match, the 2nd should be the MS amount), and said amount is a valid int
		if (slowByMatch.Success && slowByMatch.Groups.Count >= 2 && int.TryParse(slowByMatch.Groups[1].ValueSpan, out var slowByAmountMs))
			// Then simulate some async delay before executing
			await Task.Delay(slowByAmountMs, cancellationToken);

		return result;
	}
}
