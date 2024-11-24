namespace Tyne.Blazor.Filtering.Controllers;

/// <summary>
///     Non-generic base for <see cref="IFilterController{TValue}"/>.
/// </summary>
public interface IFilterController
{
    /// <summary>
    ///     When attached to a context, this is invoked when the filter this controller is attached to updates its state.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the controller handling a state change.</returns>
    /// <remarks>
    ///     This is only invoked for non-value changes to the state.
    /// </remarks>
    public Task OnStateChangedAsync();
}
