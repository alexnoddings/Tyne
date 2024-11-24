using Microsoft.AspNetCore.Components;
using Tyne.Blazor.Filtering.Context;
using Tyne.Blazor.Filtering.Values;

namespace Tyne.Blazor.Filtering.Controllers;

/// <summary>
///     The context passed to the <see cref="RenderFragment"/> content
///     rendered inside of a <see cref="TyneCustomFilterController{TRequest, TValue}"/>.
/// </summary>
/// <typeparam name="TValue">The type the filter values manage.</typeparam>
public interface ITyneCustomFilterContext<TValue>
{
    /// <summary>
    ///     The handle from attaching to the <see cref="IFilterContext{TRequest}"/>.
    /// </summary>
    public IFilterControllerHandle<TValue> Handle { get; }

    /// <inheritdoc cref="IFilterValue{TValue}.Value"/>
    public TValue? Value { get; }

    /// <summary>
    ///     Sets the value of the attached filter's <typeparamref name="TValue"/> to <paramref name="newValue"/>.
    /// </summary>
    /// <param name="newValue">The new <typeparamref name="TValue"/>.</param>
    /// <returns>A <see cref="Task"/> representing the value being set.</returns>
    /// <remarks>
    ///     <para>
    ///         This is a convenient shorthand to access <see cref="Handle"/>.
    ///     </para>
    /// </remarks>
    public Task SetFilterValueAsync(TValue? newValue);

    /// <summary>
    ///     Clears the value of the attached filter's <typeparamref name="TValue"/>.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the value being cleared.</returns>
    /// <remarks>
    ///     <para>
    ///         This is a convenient shorthand to access <see cref="Handle"/>.
    ///     </para>
    /// </remarks>
    public Task ClearFilterValueAsync();
}
