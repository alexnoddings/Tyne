namespace Tyne.Queries;

/// <summary>
///		How to order search query results.
/// </summary>
public record SearchQueryOrder
{
	/// <summary>
	///		The property to order by. This should not be null; instead, provide a null value for <see cref="SearchQueryOrder"/>.
	/// </summary>
	public string By { get; }

	/// <summary>
	///		Whether to order by descending.
	/// </summary>
	/// <remarks>
	///		When <see langword="true"/>, results will be sorted in descending order.
	///		When <see langword="false"/>, results will be sorted in ascending order.
	/// </remarks>
	public bool IsDescending { get; }

	public SearchQueryOrder(string by, bool isDescending)
	{
		By = by ?? throw new ArgumentNullException(nameof(by));
		IsDescending = isDescending;
	}
}
