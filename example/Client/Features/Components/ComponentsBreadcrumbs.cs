using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tyne.Aerospace.Client.Infrastructure;

namespace Tyne.Aerospace.Client.Features.Components;

public class ComponentsBreadcrumbs<T> : TyneBreadcrumbs
{
    protected override IEnumerable<BreadcrumbItem> GetParents() =>
        base
        .GetParents()
        .Append(new("Components", "./components"));

    public override Task SetParametersAsync(ParameterView parameters)
    {
        PageName = typeof(T).Name;
        return base.SetParametersAsync(parameters);
    }
}
