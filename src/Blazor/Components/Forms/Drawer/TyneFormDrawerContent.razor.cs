using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Tyne.Blazor;

[CascadingTypeParameter(nameof(TModel))]
public sealed partial class TyneFormDrawerContent<TModel> : ComponentBase, ITyneFormDrawerContent<TModel>, IDisposable
{
    [Parameter, EditorRequired]
    public ITyneForm<TModel> Form { get; set; } = null!;

    [Parameter]
    public RenderFragment<TModel?>? Header { get; set; }

    [Parameter]
    public RenderFragment? Loading { get; set; }

    [Parameter, EditorRequired]
    public RenderFragment<TModel> Body { get; set; } = null!;

    [Parameter]
    public RenderFragment<TModel?>? Footer { get; set; }

    [Parameter]
    public RenderFragment? CloseButton { get; set; }

    [Parameter]
    public Anchor Anchor { get; set; } = Anchor.End;

    [Parameter]
    public int Elevation { get; set; } = 1;

    [Parameter]
    public DrawerVariant Variant { get; set; } = DrawerVariant.Temporary;

    [Parameter]
    public bool Overlay { get; set; }

    [Parameter]
    public string Width { get; set; } = "360px";

    [Parameter]
    public FormValidationEvents ValidationEvents { get; set; } = FormValidationEvents.Default;

    private ITyneForm<TModel>? _oldPanel;
    private IDisposable? _panelAttachment;

    protected override void OnParametersSet()
    {
        if (_oldPanel == Form)
            return;

        _oldPanel = Form;
        _panelAttachment?.Dispose();

        if (Form is not null)
            _panelAttachment = Form.Attach(() => InvokeAsync(StateHasChanged));
    }

    private async Task TryChangeOpenAsync(bool shouldBeOpen)
    {
        if (shouldBeOpen)
            return;

        if (Form.State is FormState.Closed)
            return;

        // If the panel hasn't already closed, it must be trying to close from a dismissal
        await Form.CloseAsync(FormCloseTrigger.Dismissed).ConfigureAwait(true);
    }

    public void Dispose()
    {
        _panelAttachment?.Dispose();
        _panelAttachment = null;
    }
}
