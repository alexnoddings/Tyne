namespace Tyne.Blazor;

public interface ITyneFormDrawerContent<TModel>
{
    public ITyneForm<TModel> Form { get; }
}
