namespace Tyne.Blazor.Filtering.Controllers;

/// <summary>
///     A filter controller.
/// </summary>
/// <typeparam name="TValue">The type the filter value manages.</typeparam>
public interface IFilterController<in TValue>
{
    /// <summary>
    ///     When attached to a context, this is invoked when the <typeparamref name="TValue"/> this controller is attached to is updated.
    /// </summary>
    /// <param name="newValue">The new <typeparamref name="TValue"/>.</param>
    /// <returns>A <see cref="Task"/> representing the controller handling a value update.</returns>
    public Task OnValueUpdatedAsync(TValue? newValue);
}
