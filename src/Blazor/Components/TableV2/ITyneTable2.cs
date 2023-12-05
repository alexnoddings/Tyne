namespace Tyne.Blazor.Tables;

/// <summary>
///     A non-generic interface to allow consumers to call
///     <see cref="ReloadDataAsync"/> on <see cref="TyneTable2Base{TRequest, TResponse}"/>
///     without knowing it's generic signature.
/// </summary>
public interface ITyneTable2
{
    /// <summary>
    ///     Causes the data in the table to be reloaded.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the reload.</returns>
    public Task ReloadDataAsync();
}
