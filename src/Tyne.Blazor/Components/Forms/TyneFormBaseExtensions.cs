namespace Tyne.Blazor;

public static class TyneFormBaseExtensions
{
    public static Task OpenAsync<TModel>(this TyneFormBase<Unit, TModel> form)
    {
        ArgumentNullException.ThrowIfNull(form);
        return form.OpenAsync(Unit.Value);
    }
}
