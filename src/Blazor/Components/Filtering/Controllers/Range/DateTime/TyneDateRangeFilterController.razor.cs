using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Tyne.Blazor.Filtering.Controllers;

/// <summary>
///     A controller which attaches to a minimum and maximum <see cref="DateTime"/> property.
/// </summary>
/// <inheritdoc/>
public partial class TyneDateRangeFilterController<TRequest>
{
    /// <summary>
    ///     A label which the <see cref="MudDateRangePicker"/> should use.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    ///     A <see cref="MudBlazor.DateRange"/> of the min and max values.
    /// </summary>
    protected DateRange DateRange => new(Min, Max);

    /// <summary>
    ///     Sets the values of the attached min and max filter's <see cref="DateTime"/>.
    /// </summary>
    /// <param name="min">The new minimum <see cref="DateTime"/>.</param>
    /// <param name="max">The new maximum <see cref="DateTime"/>.</param>
    /// <returns>A <see cref="Task"/> representing the values being set.</returns>
    /// <remarks>
    ///     <para>
    ///         This wraps value setting in a batch update on the context.
    ///         This is required to ensure values are properly set - you should not
    ///         update the min or max value at the same time outside of a batch update.
    ///     </para>
    ///     <para>
    ///         This sets <paramref name="min"/>'s time to midnight (00:00:00), and <paramref name="max"/>'s time to 1 tick to midnight (23:59:59).
    ///         This makes the filter behave like [min, max] rather than the default behaviour of [min, max).
    ///     </para>
    /// </remarks>
    protected override Task SetFilterValuesAsync(DateTime? min, DateTime? max)
    {
        // MudDateRangePicker's Min and Max only have the date component set
        // Ensure min is the minimum value on min's date
        min = min?.Date;
        // Ensure max is the maximum value on max's date (the last tick before the next day)
        max = max?.Date.AddDays(1).AddTicks(-1);

        return base.SetFilterValuesAsync(min, max);
    }

    /// <summary>
    ///     Sets attached min and max filters value to be <paramref name="dateRange"/>.
    /// </summary>
    /// <param name="dateRange">The new <see cref="MudBlazor.DateRange"/>.</param>
    /// <returns>A <see cref="Task"/> representing the values being set.</returns>
    /// <remarks>
    ///     <para>
    ///         This is a convenient shorthand to call <see cref="SetFilterValuesAsync(DateTime?, DateTime?)"/>
    ///         using a <see cref="MudBlazor.DateRange"/>.
    ///     </para>
    /// </remarks>
    protected Task SetFilterValuesAsync(DateRange? dateRange) =>
        SetFilterValuesAsync(dateRange?.Start, dateRange?.End);

    /// <summary>
    ///     Clears the min and max filters value.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the values being cleared.</returns>
    protected Task ClearFilterValuesAsync() =>
        SetFilterValuesAsync(null);
}
