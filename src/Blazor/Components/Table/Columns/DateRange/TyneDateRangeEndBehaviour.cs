namespace Tyne.Blazor;

/// <summary>
///     How the max <see cref="DateTime"/> should behave for <see cref="TyneDateRangeColumn{TRequest, TResponse}"/>s.
/// </summary>
public enum TyneDateRangeMaxBehaviour
{
    /// <summary>
    ///     The max <see cref="DateTime"/> will be treated as 1 microsecond before midnight on the selected maximum date.
    /// </summary>
    /// <remarks>
    ///     For example, <c>2023/04/23 11:00:00</c> will be included when <c>2023/04/20 -> 2023/04/23</c> is selected.
    /// </remarks>
    Inclusive,

    /// <summary>
    ///     The max <see cref="DateTime"/> will be treated as midnight at the start of the selected maximum date.
    /// </summary>
    /// <remarks>
    ///     For example, <c>2023/04/23 11:00:00</c> will NOT be included when <c>2023/04/20 -> 2023/04/23</c> is selected.
    /// </remarks>
    Exclusive
}
