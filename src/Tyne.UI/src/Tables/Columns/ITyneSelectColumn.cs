namespace Tyne.UI.Tables;

public interface ITyneSelectColumn<TValue>
{
	public IDisposable RegisterSelectValue(TyneSelectValue<TValue?> selectValue);
}
