using System.Text.Json.Serialization;

namespace Tyne.Searching;

/// <summary>
///     Results returned by a search.
/// </summary>
/// <typeparam name="T">The type of results.</typeparam>
[JsonConverter(typeof(SearchResultsConverterFactory))]
public class SearchResults<T> : List<T>
{
    /// <summary>
    ///     The <b>total</b> number of results available.
    /// </summary>
    /// <remarks>
    ///     This is <b>not</b> the number of results returned.
    ///     That is provided by <see cref="List{T}.Count"/>.
    /// </remarks>
    public int TotalCount { get; }

    /// <summary>
    ///     Initialises a new <see cref="SearchResults{T}"/>.
    /// </summary>
    /// <param name="collection">A collection of <typeparamref name="T"/>s to create the results from.</param>
    /// <param name="totalCount">The total number of results available.</param>
    /// <exception cref="ArgumentNullException">When <paramref name="collection"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">When <paramref name="totalCount"/> is less than <c>0</c>.</exception>
    public SearchResults(IEnumerable<T> collection, int totalCount) : base(collection)
    {
        ArgumentNullException.ThrowIfNull(collection);

        if (totalCount < 0)
            throw new ArgumentOutOfRangeException(nameof(totalCount), "Total count cannot be less than 0.");

        if (totalCount < Count)
            throw new ArgumentOutOfRangeException(nameof(totalCount), "Total count cannot be less than the collection's count.");

        TotalCount = totalCount;
    }
}
