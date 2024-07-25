using MudBlazor;

namespace Tyne.Aerospace.Client.Features.Systems.Tables;

public class TablesBreadcrumbs : SystemsBreadcrumbs
{
    protected override IEnumerable<BreadcrumbItem> GetParents() =>
        base
        .GetParents()
        .Append(new("Tables", href: "./systems/tables"));
}
