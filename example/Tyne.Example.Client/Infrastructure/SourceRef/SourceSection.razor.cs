using Microsoft.AspNetCore.Components;

namespace Tyne.Example.Client.Infrastructure;

public partial class SourceSection
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
