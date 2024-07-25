using Microsoft.AspNetCore.Components;

namespace Tyne.Aerospace.Client.Infrastructure;

public partial class LinkSection
{
    [Parameter]
    public RenderFragment ChildContent { get; set; } = null!;
}
