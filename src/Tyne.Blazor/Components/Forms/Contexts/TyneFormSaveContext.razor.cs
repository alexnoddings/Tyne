using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor;

public partial class TyneFormSaveContext
{
    [Parameter]
    public RenderFragment<ITyneFormSaveContext>? ChildContent { get; set; }

    [Parameter]
    public RenderFragment<SaveForm>? Supported { get; set; }

    [Parameter]
    public RenderFragment? NotSupported { get; set; }

    private Context? _context;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Form is { } form)
            _context = new(form);
    }

    private sealed class Context : ITyneFormSaveContext
    {
        private readonly ITyneForm _form;

        public Context(ITyneForm form)
        {
            _form = form;
        }

        public bool CanSave => _form.State is TyneFormState.Supports.Saving;

        public Task SaveAsync()
        {
            if (_form.State is TyneFormState.Supports.Saving saving)
                return saving.SaveAsync();

            return Task.CompletedTask;
        }
    }
}
