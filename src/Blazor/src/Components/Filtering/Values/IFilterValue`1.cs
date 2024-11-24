using Tyne.Blazor.Filtering.Controllers;

namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     A filter value.
/// </summary>
/// <typeparam name="TValue">The type of value being managed.</typeparam>
public interface IFilterValue<TValue>
{
    /// <summary>
    ///     The current <typeparamref name="TValue"/>.
    /// </summary>
    /// <remarks>
    ///     This may be <see langword="null"/> if no value is set,
    ///     or if it is being loaded asynchronously (e.g. from an API).
    /// </remarks>
    public TValue? Value { get; }

    /// <summary>
    ///     Updates <see cref="Value"/> with <paramref name="newValue"/>.
    /// </summary>
    /// <param name="newValue">The new <typeparamref name="TValue"/> being set.</param>
    /// <returns>A <see cref="Task"/> representing the operation.</returns>
    /// <remarks>
    ///     This will notify all attached controllers via <see cref="IFilterController{TValue}.OnValueUpdatedAsync(TValue)"/>.
    /// </remarks>
    public Task SetValueAsync(TValue? newValue);

    /// <summary>
    ///     Clears <see cref="Value"/>.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the operation.</returns>
    /// <remarks>
    ///     This will notify all attached controllers via <see cref="IFilterController{TValue}.OnValueUpdatedAsync(TValue)"/>.
    /// </remarks>
    public Task ClearValueAsync();
}
