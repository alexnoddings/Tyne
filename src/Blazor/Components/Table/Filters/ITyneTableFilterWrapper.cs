namespace Tyne.Blazor;

/// <summary>
///     A wrapper used by <see cref="TyneTableFilter{TRequest, TValue}"/>
///     to provide child content with access to getting and setting it's value.
/// </summary>
/// <typeparam name="TValue">The type of value the filter holds.</typeparam>
public interface ITyneTableFilterWrapper<TValue>
{
    /// <summary>
    ///     Accesses the <typeparamref name="TValue"/>.
    /// </summary>
    public TValue? Value { get; }

    // This is provided separately rather than having SetValueAsync accept a default cancellation token
    // as Blazor's EventCallbackFactory can't implicitly convert that method signature.
    /// <inheritdoc cref="SetValueAsync(TValue?, CancellationToken)"/>
    public Task SetValueAsync(TValue? newValue);

    /// <summary>
    ///     Updates the <typeparamref name="TValue"/> with <paramref name="newValue"/>.
    /// </summary>
    /// <param name="newValue">
    ///     The new <typeparamref name="TValue"/> to update the filter with.
    /// </param>
    /// <param name="cancellationToken">
    ///     Optionally, a <see cref="CancellationToken"/>.
    /// </param>
    /// <returns>
    ///     A <see cref="Task"/> that represents the asynchronous operation.
    /// </returns>
    public Task SetValueAsync(TValue? newValue, CancellationToken cancellationToken);
}
