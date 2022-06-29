namespace Tyne.UI.Tables;

public interface IFilteredColumn<in TSearch>
{
	public void Prepare(TSearch search);
}
