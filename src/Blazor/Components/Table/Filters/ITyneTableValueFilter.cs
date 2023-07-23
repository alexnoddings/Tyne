namespace Tyne.Blazor;

/// <summary>
///     A filter in a <see cref="ITyneTable"/> which holds a <typeparamref name="TValue"/>.
/// </summary>
/// <typeparam name="TValue">The type of value this filter holds.</typeparam>
public interface ITyneTableValueFilter<TValue>
{
    /// <summary>
    ///     The current <typeparamref name="TValue"/> of the filter.
    /// </summary>
    /// <remarks>
    ///     This may be <see langword="null" />.
    /// </remarks>
    public TValue? Value { get; }

    /// <summary>
    ///     Sets <see cref="Value"/> to <paramref name="newValue"/>.
    /// </summary>
    /// <param name="newValue">
    ///     The new <typeparamref name="TValue"/> to set as <see cref="Value"/>. This may be <see langword="null" />.
    /// </param>
    /// <param name="isSilent">
    ///     If set silently (<see langword="true"/>), then the filter won't trigger any update functions, such as:
    ///     <list type="bullet">
    ///         <item>reloading the table</item>
    ///         <item>synchronising the value</item>
    ///         <item>persisting the value</item>
    ///     </list>
    /// </param>
    /// <param name="cancellationToken">
    ///     Optionally, a <see cref="CancellationToken"/>.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> that represents the asynchronous operation.
    ///     The result of this task will be <see langword="true" /> if the <see cref="Value"/> was updated, otherwise <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     This may not update <see cref="Value"/> (and thus return <see langword="false"/>)
    ///     if the implementer considers <paramref name="newValue"/> to already equal <see cref="Value"/>.
    /// </remarks>
    public Task<bool> SetValueAsync(TValue? newValue, bool isSilent, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Clears the value of <see cref="Value"/>.
    /// </summary>
    /// <param name="cancellationToken">
    ///     Optionally, a <see cref="CancellationToken"/>.
    /// </param>
    /// <returns>
    ///     A <see cref="Task"/> that represents the asynchronous operation.
    ///     The result of this task will be <see langword="true" /> if the <see cref="Value"/> was cleared, otherwise <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This is usually equivalent to calling <see cref="SetValueAsync(TValue?, bool, CancellationToken)"/> with <c>(<see langword="default" />, <see langword="true" />)</c>.
    ///     </para>
    ///     <para>
    ///         Similarly, if <see cref="Value"/> is already considered to be clear then it will not be updated, and this will return <see langword="false"/>.
    ///     </para>
    /// </remarks>
    public Task<bool> ClearValueAsync(CancellationToken cancellationToken = default);
}
