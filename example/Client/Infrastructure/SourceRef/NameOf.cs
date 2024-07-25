using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Tyne.Aerospace.Client.Infrastructure;

public class NameOf : ComponentBase
{
    [Parameter]
    public Type? T { get; set; }

    [Parameter]
    public string? M { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private string? _identifier = "";

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenElement(0, "code");

        if (ChildContent is not null)
            builder.AddContent(1, ChildContent);
        else
            builder.AddContent(2, _identifier);

        builder.CloseElement();
    }

    protected override void OnParametersSet()
    {
        if (ChildContent is not null)
        {
            if (T is not null)
                throw new InvalidOperationException($"'{nameof(T)}' cannot be set if a '{nameof(ChildContent)}' is provided.");

            if (M is not null)
                throw new InvalidOperationException($"'{nameof(M)}' cannot be set if a '{nameof(ChildContent)}' is provided.");

            _identifier = null;
            return;
        }

        if (M is null && T is null)
            throw new InvalidOperationException($"One or both of '{nameof(T)}'/'{nameof(M)}' must be set if '{nameof(ChildContent)}' is not provided.");

        var typeName = T?.Name;
        if (typeName is not null)
        {
            var typeGenericMarker = typeName.IndexOf('`', StringComparison.OrdinalIgnoreCase);
            if (typeGenericMarker != -1)
                typeName = typeName[..typeGenericMarker];
        }

        var memberName = M;

        _identifier = (typeName, methodName: memberName) switch
        {
            (not null, not null) => typeName + '.' + memberName,
            (not null, null) => typeName,
            (null, not null) => memberName,
            _ => "???",
        };
    }
}
