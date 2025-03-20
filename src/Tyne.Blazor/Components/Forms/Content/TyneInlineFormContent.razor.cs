using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor;

[CascadingTypeParameter(nameof(TParams))]
[CascadingTypeParameter(nameof(TModel))]
[CascadingTypeParameter(nameof(TError))]
public sealed partial class TyneInlineFormContent<TParams, TModel, TError> : ComponentBase, IDisposable
{
    [CascadingParameter]
    public ITyneForm? Form { get; set; }

    [Parameter]
    public RenderFragment<TError>? Error { get; set; }

    [Parameter]
    public RenderFragment? Initialising { get; set; }

    [Parameter]
    public RenderFragment? Inactive { get; set; }

    [Parameter]
    public RenderFragment? Loading { get; set; }

    [Parameter]
    public RenderFragment<TyneFormState.Is.Ready<TParams, TModel, TError>>? Ready { get; set; }

    private IDisposable? _hook;

    protected override void OnInitialized()
    {
        if (Form is not null)
            _hook = Form.WatchForStateChanges(() => InvokeAsync(StateHasChanged));
    }

    protected override void OnParametersSet()
    {
        if (Form is null)
            throw new InvalidOperationException($"{nameof(TyneInlineFormContent<,,>)} requires a cascading parameter of type {nameof(ITyneForm)}.");
    }

    public void Dispose() => _hook?.Dispose();
}
