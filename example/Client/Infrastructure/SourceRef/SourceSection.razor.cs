using Microsoft.AspNetCore.Components;

namespace Tyne.Aerospace.Client.Infrastructure;

public partial class SourceSection
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
