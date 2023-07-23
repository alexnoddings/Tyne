using System.Reflection;

namespace Tyne.Blazor;

/// <summary>
///     The non-generic version of <see cref="ITyneTableSyncedFilter{TValue}"/>
///     which exposes <see cref="SyncKey"/> without needing a generic parameter.
/// </summary>
/// <inheritdoc />
public interface ITyneTableSyncedFilter
{
    /// <summary>
    ///     A key to synchronise <see cref="ITyneTableValueFilter{TValue}.Value"/> with other <see cref="ITyneTableSyncedFilter{TValue}"/>s..
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         If <see cref="TyneTableKey.IsEmpty"/>, synchronisation will be disabled.
    ///     </para>
    ///     <para>
    ///         For columns which implement a <c>For="..."</c> property,
    ///         <see cref="TyneTableKey.From(string?, PropertyInfo?)"/> should be used.
    ///         Otherwise, <see cref="TyneTableKey.From(string?)"/> should be used if
    ///         there are no properties known about.
    ///     </para>
    /// </remarks>
    public TyneTableKey SyncKey { get; }
}
