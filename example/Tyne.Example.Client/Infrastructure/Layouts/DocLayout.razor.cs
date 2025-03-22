using System.Reflection;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tyne.Example.Client.Infrastructure.Layouts;

namespace Tyne.Example.Client.Infrastructure;

public partial class DocLayout
{
    [CascadingParameter]
    protected RouteData RouteData { get; init; } = null!;

    private MaxWidth DefaultMaxWidth => MudBlazor.MaxWidth.Medium;
    private MaxWidth? MaxWidth =>
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

    protected virtual RenderFragment RenderTitle => b => b.AddContent(0, PageTitle);

    private bool DisableBack =>
        RouteData
            .PageType
            .GetCustomAttribute<DisableBackLinkAttribute>() is not null;
}
