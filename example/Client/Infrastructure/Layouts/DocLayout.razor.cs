using System.Reflection;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Tyne.Aerospace.Client.Infrastructure;

public partial class DocLayout
{
    [CascadingParameter]
    protected RouteData RouteData { get; init; } = null!;

    protected virtual MaxWidth DefaultMaxWidth => MudBlazor.MaxWidth.Medium;
    protected virtual MaxWidth? MaxWidth =>
        RouteData
        .PageType
        .GetCustomAttribute<PageMaxWidthAttribute>()
        ?.MaxWidth;

    protected virtual string PageTitle =>
        RouteData
        .PageType
        .GetCustomAttribute<PageTitleAttribute>()
        ?.Title
        ?? string.Empty;

    protected virtual RenderFragment RenderBreadcrumbs() => EmptyRenderFragment.Instance;
    protected virtual RenderFragment RenderTitle() => builder => builder.AddContent(0, PageTitle);
    protected virtual RenderFragment RenderBanner() => EmptyRenderFragment.Instance;
}
