using MudBlazor;
using Tyne.Aerospace.Client.Infrastructure;

namespace Tyne.Aerospace.Client.Features.Systems;

public class SystemsBreadcrumbs : TyneBreadcrumbs
{
    protected override IEnumerable<BreadcrumbItem> GetParents() =>
        base
        .GetParents()
        .Append(new("Systems", "./systems"));
}
