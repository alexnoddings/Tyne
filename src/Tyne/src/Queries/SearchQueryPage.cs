namespace Tyne.Queries;

/// <summary>
///		How to paginate search query results.
/// </summary>
/// <param name="Index">The 0-based page index to return.</param>
/// <param name="Size">The size of page to return.</param>
public record SearchQueryPage(int Index, int Size)
{
	/// <summary>
	///		The default page to return.
	/// </summary>
	public static SearchQueryPage Default => new(0, 10);
}
