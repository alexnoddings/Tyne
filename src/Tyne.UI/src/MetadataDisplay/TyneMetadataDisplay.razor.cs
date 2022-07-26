using Microsoft.AspNetCore.Components;
using Tyne.Results;

namespace Tyne.UI;

public partial class TyneMetadataDisplay
{
	[Parameter]
	public bool Disabled { get; set; }

	[Parameter]
	public bool Dismissible { get; set; } = true;

	[Parameter, EditorRequired]
	public List<IMetadata>? Metadata { get; set; }
}
