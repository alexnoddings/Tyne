using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Hosting;

namespace Tyne.UI;

public partial class Environment : ComponentBase
{
	[Inject]
	private IHostEnvironment HostEnvironment { get; set; } = default!;

	[Parameter]
	public RenderFragment<string>? ChildContent { get; set; }

	[Parameter, EditorRequired]
	public EnvironmentMode Mode { get; set; } = EnvironmentMode.Include;

	[Parameter, EditorRequired]
	public string Filter { get; set; } = string.Empty;

	private string[] EnvironmentNames { get; set; } = Array.Empty<string>();

	protected override void OnParametersSet()
	{
		EnvironmentNames = Filter.ToLowerInvariant().Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
	}

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		var hostEnvironmentName = HostEnvironment.EnvironmentName;
		var environmentNameMatches = EnvironmentNames.Contains(hostEnvironmentName.ToLowerInvariant());
		if (environmentNameMatches && Mode == EnvironmentMode.Include)
			builder.AddContent(0, ChildContent, hostEnvironmentName);
		else if (!environmentNameMatches && Mode == EnvironmentMode.Exclude)
			builder.AddContent(0, ChildContent, hostEnvironmentName);
	}
}
