using System.Data.Common;
using Tyne.EntityFramework;

namespace Microsoft.EntityFrameworkCore;

/// <summary>
///     Extension method for adding a delay to <see cref="DbCommand"/>s.
/// </summary>
public static class DbSlowByExtensions
{
    /// <summary>
    ///     Enables slowing-down of <see cref="DbCommands"/> executed by the <see cref="DbContext"/> using <paramref name="optionsBuilder"/>. 
    /// </summary>
    /// <param name="optionsBuilder">The builder being used to configure the context.</param>
    /// <param name="enableDebugSlowBy">If <see langword="true" />, then slowing-down will be enabled.</param>
    /// <returns>The <paramref name="optionsBuilder"/> to allow for chaining.</returns>
    /// <remarks>
    ///     Queries can be slowed down by using <see cref="EntityFrameworkQueryableExtensions.TagWith"/> before executing them.
    ///     Tag queries with <c>.TagWith("SLOW BY {amount}ms")</c> to have an artificial delay of <c>{amount}</c> miliseconds introduced before the query is executed.
    ///     Only the first <see cref="EntityFrameworkQueryableExtensions.TagWith"/> will be respected; any further calls are ignored.
    /// </remarks>
    public static DbContextOptionsBuilder EnableDebugSlowBy(this DbContextOptionsBuilder optionsBuilder, bool enableDebugSlowBy = true)
    {
        if (enableDebugSlowBy)
            optionsBuilder.AddInterceptors(new DbSlowByCommandInterceptor());

        return optionsBuilder;
    }
}
