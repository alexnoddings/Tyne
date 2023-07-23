namespace Tyne.Blazor;

/// <summary>
///     A <see cref="ITyneTableValueFilter{TValue}"/> whose <see cref="ITyneTableValueFilter{TValue}.Value"/>
///     should be synchronised with other filters with the same <see cref="ITyneTablePersistedFilter.PersistKey"/>.
/// </summary>
/// <inheritdoc />
public interface ITyneTableSyncedFilter<TValue> :
    ITyneTableValueFilter<TValue>,
    ITyneTableSyncedFilter
{
}
