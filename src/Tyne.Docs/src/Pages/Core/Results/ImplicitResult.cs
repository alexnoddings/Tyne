using Tyne.Docs.SourceGen;
using Tyne.Results;

namespace Tyne.Docs.Pages.Core.Results;

[Sample]
public static partial class CreateLocation
{
	public static Result<int> GetSomeCount()
	{
		return 12;
	}
}
