using MudBlazor;

namespace Tyne.Aerospace.Client.Features.Systems.Filtering;

public class FilteringBreadcrumbs : SystemsBreadcrumbs
{
    protected override IEnumerable<BreadcrumbItem> GetParents() =>
        base
        .GetParents()
        .Append(new("Filtering", href: "./systems/filtering"));
}
