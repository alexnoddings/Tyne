using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor.Filtering.Controllers;

/// <summary>
///     The context passed to the <see cref="RenderFragment"/> content
///     rendered inside of a <see cref="TyneCustomMinMaxFilterController{TRequest, TValue}"/>.
/// </summary>
/// <typeparam name="TValue">The type the filter values manage.</typeparam>
public interface ITyneCustomMinMaxFilterContext<TValue>
{
    /// <summary>
    ///     The <typeparamref name="TValue"/> of <see cref="MinHandle"/>'s value.
    /// </summary>
    public TValue? Min { get; }
    /// <summary>
    ///     The handle from attaching to the min property.
    /// </summary>
    public IFilterControllerHandle<TValue?> MinHandle { get; }

    /// <summary>
    ///     The <typeparamref name="TValue"/> of <see cref="MaxHandle"/>'s value.
    /// </summary>
    public TValue? Max { get; }
    /// <summary>
    ///     The handle from attaching to the max property.
    /// </summary>
    public IFilterControllerHandle<TValue?> MaxHandle { get; }

    /// <summary>
    ///     Sets the values of the attached min and max filter's <typeparamref name="TValue"/>.
    /// </summary>
    /// <param name="min">The new minimum <typeparamref name="TValue"/>.</param>
    /// <param name="max">The new maximum <typeparamref name="TValue"/>.</param>
    /// <returns>A <see cref="Task"/> representing the values being set.</returns>
    /// <remarks>
    ///     This wraps value setting in a batch update on the context.
    ///     This is required to ensure values are properly set - you should not
    ///     update the min or max value at the same time outside of a batch update.
    /// </remarks>
    public Task SetValuesAsync(TValue? min, TValue? max);
}
