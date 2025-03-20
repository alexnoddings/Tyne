using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor;

public partial class TyneFormProcessingContext
{
    [Parameter]
    public RenderFragment<ITyneFormProcessingContext>? ChildContent { get; set; }

    [Parameter]
    public RenderFragment? Processing { get; set; }

    [Parameter]
    public RenderFragment? NotProcessing { get; set; }

    private Context? _context;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Form is { } form)
            _context = new(form);
    }

    private sealed class Context : ITyneFormProcessingContext
    {
        private readonly ITyneForm _form;

        public Context(ITyneForm form)
        {
            _form = form;
        }

        public bool IsProcessing => _form.State
            is TyneFormState.Initialising
            or TyneFormState.Loading
            or TyneFormState.Saving;
    }
}
