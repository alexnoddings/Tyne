using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Utilities;

namespace Tyne.Aerospace.Client.Features.Dashboard;

public sealed partial class DashboardCardContent
{
    [Parameter]
    public RenderFragment ChildContent { get; set; } = EmptyRenderFragment.Instance;

    [Parameter]
    public string Icon { get; set; } = string.Empty;

    [Parameter]
    public Color Colour { get; set; }

    [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Class names are lower case.")]
    private string ColourName =>
        Colour
        .ToString()
        .ToLowerInvariant();

    private string OuterClassName =>
        new CssBuilder("pa-0 pr-1 pb-1")
        .AddClass($"mud-theme-{ColourName}")
        .Build();
}
