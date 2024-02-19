using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Tyne.Aerospace.Client.Infrastructure;

public partial class TyneBreadcrumbs : ComponentBase
{
    [Parameter]
    public string PageName { get; set; } = string.Empty;

    private List<BreadcrumbItem> Breadcrumbs { get; set; } = null!;

    protected virtual IEnumerable<BreadcrumbItem> GetParents() =>
        Enumerable.Empty<BreadcrumbItem>();

    protected override void OnInitialized()
    {
        Breadcrumbs = new(GetParents())
        {
            new(PageName, href: null)
        };
    }

    protected override void OnParametersSet()
    {
        Breadcrumbs[^1] = new(PageName, href: null);
    }
}
