using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Options;

namespace Tyne.UI;

public class TyneTitle : ComponentBase
{
    [Parameter]
    public string? Value { get; set; }

    [Inject]
    private IOptions<TitleOptions> Options { get; set; } = default!;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenComponent<PageTitle>(0);
        builder.AddAttribute(1, nameof(PageTitle.ChildContent), (RenderFragment)BuildPageTitleChildContentRenderTree);
        builder.CloseElement();
    }

    private void BuildPageTitleChildContentRenderTree(RenderTreeBuilder builder)
    {
        TitleOptions options = Options.Value ?? new();

        string pageTitle;
        if (string.IsNullOrWhiteSpace(Value))
            pageTitle = options.AppName;
        else if (options.IsSuffix)
            pageTitle = Value + options.Divider + options.AppName;
        else
            pageTitle = options.AppName + options.Divider + Value;

        builder.AddContent(0, pageTitle);
    }
}
