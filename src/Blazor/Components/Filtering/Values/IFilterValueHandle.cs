using Tyne.Blazor.Filtering.Context;

namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     A handle given to a <see cref="IFilterValue{TValue}"/> when it attaches to an <see cref="IFilterContext{TRequest}"/>.
/// </summary>
/// <typeparam name="TValue">The type the filter value manages.</typeparam>
/// <remarks>
///     This handle should be disposed once the <see cref="IFilterValue{TValue}"/>
///     goes out of scope to ensure it is detached from the <see cref="IFilterContext{TRequest}"/>.
/// </remarks>
public interface IFilterValueHandle<in TValue> : IDisposable
{
    /// <summary>
    ///     Instructs the <see cref="IFilterContext{TRequest}"/> to reload the data being filtered.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the data being reloaded.</returns>
    public Task ReloadDataAsync();

    /// <summary>
    ///     Notifies the <see cref="IFilterContext{TRequest}"/>
    ///     (and any controllers attached to this value)
    ///     that this value has a <paramref name="newValue"/>.
    /// </summary>
    /// <param name="newValue">The new <typeparamref name="TValue"/>.</param>
    /// <returns>A <see cref="Task"/> representing the new value notification being handled.</returns>
    public Task NotifyValueUpdatedAsync(TValue? newValue);
}
