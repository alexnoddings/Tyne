namespace Tyne.Blazor;

public interface ITyneTablePersistedFilter<TValue> : ITyneTableValueFilter<TValue>
{
    public TyneFilterPersistKey PersistKey { get; }
}
