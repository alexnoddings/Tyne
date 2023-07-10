using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Tyne.Blazor;

public class TyneLocalisedDateTime : TyneLocalisedDateTimeBase
{
    [Parameter]
    public RenderFragment<DateTimeOffset> ChildContent { get; set; } = EmptyRenderFragment.For<DateTimeOffset>();

    [Parameter]
    public RenderFragment? Loading { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        if (DateTimeLocal is null)
            builder.AddContent(0, Loading);
        else
            builder.AddContent(1, ChildContent, DateTimeLocal.Value);
    }
}
