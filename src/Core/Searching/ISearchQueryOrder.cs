namespace Tyne.Searching;

/// <summary>
///     How to order search query results.
/// </summary>
public interface ISearchQueryOrder
{
    /// <summary>
    ///		The property to order by.
    /// </summary>
    public string? OrderBy { get; set; }

    /// <summary>
    ///		Whether to order the results descending.
    /// </summary>
    /// <remarks>
    ///		When <see langword="true"/>, results will be sorted in descending order.
    ///		When <see langword="false"/>, results will be sorted in ascending order.
    /// </remarks>
    public bool OrderByDescending { get; set; }
}
