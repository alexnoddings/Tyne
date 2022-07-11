using Tyne.Docs.SourceGen;
using Tyne.Utilities;

namespace Tyne.Docs.Pages.Core.Utilities;

[Sample]
public static partial class Counter
{
	public static int Count { get; private set; } = 0;

	public static IDisposable Increment()
	{
		// Note that this demo is not thread-safe
		Count++;
		return new DisposableAction(() => Count--);
	}
}
