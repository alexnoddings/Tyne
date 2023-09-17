using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Tyne.Aerospace.Data;

namespace Tyne.Aerospace.Client.Infrastructure;

public static class SqliteWasmDbContextFactoryExtensions
{
    /// <summary>
    ///     Forces <paramref name="factory"/> to sync <paramref name="dbContext"/> with the browser.
    /// </summary>
    /// <param name="factory">The factory which <paramref name="dbContext"/> came from.</param>
    /// <param name="dbContext">The <see cref="AppDbContext"/> to sync to the browser.</param>
    /// <returns>
    ///     A <see cref="Task"/> that represents the asynchronous sync operation.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         ExecuteDelete executes immediately against the database, it doesn't wait for SaveChanges.
    ///         However, in browser SQLite we only persist the DbContext when SaveChanges is called.
    ///         This forces the <paramref name="factory"/> to persist the deletion.
    ///     </para>
    ///     <para>
    ///         The SQLite DbContextFactory tries to be clever, and only writes when there are changes made.
    ///         However, it only checks SavedChangesEventArgs.EntitiesSavedCount - which is 0 as the delete execution bypasses this mechanism.
    ///         So we reflect on the <paramref name="factory"/> to trick it into persisting.
    ///         This is quite nasty, but less worse than the alternatives.
    ///     </para>
    /// </remarks>
    [SuppressMessage("Major Code Smell", "S3011: Reflection should not be used to increase accessibility of classes, methods, or fields", Justification = "This is the least worst solution.")]
    public static Task ForceSyncAsync(this IAppDbContextFactory factory, AppDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(dbContext);

        // Can't easily statically cache this with MethodHelper as Ctx_SavedChanges takes the DbContext a generic type param
        var ctxSavedChangesMethod = factory.GetType().GetMethod("Ctx_SavedChanges", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
        if (ctxSavedChangesMethod is null)
            throw new InvalidOperationException("No Ctx_SavedChanges method found on factory?");

        var parameters = new object?[]
        {
            dbContext,
            new SavedChangesEventArgs(true, 1)
        };

        ctxSavedChangesMethod.Invoke(factory, parameters);

        // None of this is async, but may be in the future
        return Task.CompletedTask;
    }
}
