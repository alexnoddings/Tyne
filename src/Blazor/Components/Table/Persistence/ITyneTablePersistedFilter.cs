using System.Reflection;

namespace Tyne.Blazor;

/// <summary>
///     A <see cref="ITyneTableValueFilter{TValue}"/> whose <see cref="ITyneTableValueFilter{TValue}.Value"/>
///     should be persisted when updated using <see cref="PersistKey"/>.
/// </summary>
/// <inheritdoc />
public interface ITyneTablePersistedFilter<TValue> : ITyneTableValueFilter<TValue>
{
    /// <summary>
    ///     A key to persist <see cref="ITyneTableValueFilter{TValue}.Value"/> as.
    /// </summary>
    /// <remarks>
    ///     For columns which implement a <c>For="..."</c> property,
    ///     <see cref="TyneFilterPersistKey.From(string?, PropertyInfo?)"/> should be used.
    ///     Otherwise, <see cref="TyneFilterPersistKey.From(string?)"/> should be used if
    ///     there are no properties known about.
    /// </remarks>
    public TyneFilterPersistKey PersistKey { get; }
}
