namespace Tyne.Blazor;

public interface ITyneSelectColumn<TValue>
{
    public IDisposable RegisterValue(TyneSelectValue<TValue?> value);
}
