using System.Reflection;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Tyne.Aerospace.Client.Infrastructure;

public abstract partial class DocLayoutBase
{
    [CascadingParameter]
    protected RouteData RouteData { get; init; } = null!;

    protected abstract string PageTitle { get; }

    protected virtual MaxWidth DefaultMaxWidth => MudBlazor.MaxWidth.Medium;

    protected abstract RenderFragment RenderBreadcrumbs();
    protected virtual RenderFragment RenderTitle() => builder => builder.AddContent(0, PageTitle);
    protected virtual RenderFragment RenderBanner() => EmptyRenderFragment.Instance;

    private MaxWidth? MaxWidth =>
        RouteData
        .PageType
        .GetCustomAttribute<PageMaxWidthAttribute>()
        ?.MaxWidth;
}
