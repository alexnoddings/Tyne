namespace Tyne.Blazor;

/// <summary>
///     A <see cref="ITyneTableValueFilter{TValue}"/> whose <see cref="ITyneTableValueFilter{TValue}.Value"/>
///     should be persisted when updated using <see cref="ITyneTablePersistedFilter.PersistKey"/>.
/// </summary>
/// <inheritdoc />
public interface ITyneTablePersistedFilter<TValue> :
    ITyneTableValueFilter<TValue>,
    ITyneTablePersistedFilter
{
}
