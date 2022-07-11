using Microsoft.AspNetCore.Components;

namespace Tyne.Docs.Shared;

public static class DocsUtilities
{
	public static RenderFragment<string> Keyword { get; } = keyword => builder =>
	{
		builder.OpenElement(0, "code");
		builder.AddContent(1, keyword);
		builder.CloseComponent();
	};
}
