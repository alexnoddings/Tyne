using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor;

public partial class TyneFormMakeInactiveContext
{
    [Parameter]
    public RenderFragment<ITyneFormMakeInactiveContext>? ChildContent { get; set; }

    [Parameter]
    public RenderFragment<MakeFormInactive>? Supported { get; set; }

    [Parameter]
    public RenderFragment? NotSupported { get; set; }

    private Context? _context;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Form is { } form)
            _context = new(form);
    }

    private sealed class Context : ITyneFormMakeInactiveContext
    {
        private readonly ITyneForm _form;

        public Context(ITyneForm form)
        {
            _form = form;
        }

        public bool CanMakeInactive => _form.State is TyneFormState.Supports.MakingInactive;

        public Task MakeInactiveAsync()
        {
            if (_form.State is TyneFormState.Supports.MakingInactive makeInactive)
                return makeInactive.MakeInactiveAsync();

            return Task.CompletedTask;
        }
    }
}
