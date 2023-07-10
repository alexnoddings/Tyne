using MudBlazor;
using Tyne.Searching;

namespace Tyne.Blazor;

/// <summary>
///     A column-based filter in a <see cref="ITyneTable"/> which can configure a <typeparamref name="TRequest"/> before it is sent.
/// </summary>
/// <typeparam name="TRequest">The type of request which this filter configures.</typeparam>
/// <remarks>
///     This exposes column-specific functionality on top of <see cref="ITyneTableRequestFilter{TRequest}"/>.
/// </remarks>
public interface ITyneFilteredColumn<in TRequest> : ITyneTableRequestFilter<TRequest>
{
    /// <summary>
    ///     <see langword="true"/> if the filter is currently visible, otherwise <see langword="false"/>.
    /// </summary>
    /// <remarks>
    ///     A filter is considered visible when it is being configured, such as when a popover is open.
    /// </remarks>
    public bool IsFilterVisible { get; set; }
    /// <summary>
    ///     <see langword="true"/> if the filter is currently active, otherwise <see langword="false"/>.
    /// </summary>
    /// <remarks>
    ///     A filter is considered active when it is able to filter table results.
    ///     This usually means it has a value set.
    /// </remarks>
    public bool IsFilterActive { get; }

    /// <summary>
    ///     The icon to display on the column header.
    /// </summary>
    /// <remarks>
    ///     This will usually change based on whether the filter is visible or active.
    /// </remarks>
    public string Icon { get; }

    /// <summary>
    ///     The <see cref="Color"/> of the <see cref="Icon"/>.
    /// </summary>
    public Color IconColour { get; }

    /// <summary>
    ///     Optionally, a property name to order the column's results by.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         If left as <see langword="null"/>, no ordering capabilities will be presented to the user for this column.
    ///     </para>
    ///     <para>
    ///         If set, ordering capabilities will be presented, which will use the value for <see cref="ISearchQueryOrder.OrderBy"/>.
    ///     </para>
    /// </remarks>
    public string? OrderByName { get; }

    /// <summary>
    ///     Clears any active value(s).
    /// </summary>
    /// <param name="cancellationToken">
    ///     Optionally, a <see cref="CancellationToken"/>.
    /// </param>
    /// <returns>
    ///     A <see cref="Task"/> that represents the asynchronous operation.
    ///     The result of this task will be <see langword="true" /> if the value(s) were cleared, otherwise <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     This should be equivalent to <see cref="ITyneTableValueFilter{TValue}.ClearValueAsync(CancellationToken)"/>
    ///     for columns which implement both.
    /// </remarks>
    public Task<bool> ClearValueAsync(CancellationToken cancellationToken = default);
}
