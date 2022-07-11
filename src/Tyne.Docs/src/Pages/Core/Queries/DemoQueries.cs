using Tyne.Docs.SourceGen;
using Tyne.Queries;

namespace Tyne.Docs.Pages.Core.Queries;

[Sample]
public static partial class DemoQueries
{
	public class SimpleQuery : BaseSearchQuery
	{
		// BaseSearchQuery handles Page and Order for you
		// public SearchQueryPage Page { get; set; } = SearchQueryPage.Default;
		// public SearchQueryOrder? Order { get; set; }
	}

	// A BaseSearchQuery with an extra property which can be filtered by in the handler
	public class FilteredQuery : BaseSearchQuery
	{
		public string? Name { get; set; }
		public int? MinItems { get; set; }
	}
}
