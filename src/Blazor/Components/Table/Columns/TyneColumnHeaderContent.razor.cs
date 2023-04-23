using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor;

[SuppressMessage("Major Code Smell", "S2326: Unused type parameters should be removed", Justification = "Used for consistency across other column code.")]
public partial class TyneColumnHeaderContent<TRequest, TResponse>
{
    [Parameter, EditorRequired]
    public ITyneFilteredColumn<TRequest> Column { get; set; } = null!;

    [Parameter]
    public RenderFragment? Header { get; set; }

    [Parameter]
    public RenderFragment? Filter { get; set; }

    private void ToggleFilterVisibility() =>
        Column.IsFilterVisible = !Column.IsFilterVisible;
}
