using Microsoft.AspNetCore.Components;

namespace Tyne.Example.Client.Components;

public partial class LinkSection
{
    [Parameter]
    public RenderFragment ChildContent { get; set; } = null!;
}
