namespace Tyne.Blazor;

public interface ITyneForm<TModel> : ITyneForm
{
    public TModel? Model { get; }
    public Result<TModel>? OpenResult { get; }
}
