using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor;

public partial class TyneFormMakeActiveContext<TModel, TError>
{
    [Parameter]
    public RenderFragment<ITyneFormMakeActiveContext<TModel, TError>>? ChildContent { get; set; }

    [Parameter]
    public RenderFragment<Func<MakeFormActive<TModel, TError>, Task>>? Supported { get; set; }

    [Parameter]
    public RenderFragment? NotSupported { get; set; }

    private Context? _context;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Form is { } form)
            _context = new(form);
    }

    private sealed class Context : ITyneFormMakeActiveContext<TModel, TError>
    {
        private readonly ITyneForm _form;

        public Context(ITyneForm form)
        {
            _form = form;
        }

        public bool CanMakeActive => _form.State is TyneFormState.Supports.MakingActive<TModel, TError>;

        public Task MakeActiveAsync(MakeFormActive<TModel, TError> makeFormActive)
        {
            if (_form.State is TyneFormState.Supports.MakingActive<TModel, TError> makeActive)
                return makeActive.MakeActiveAsync(makeFormActive);

            return Task.CompletedTask;
        }
    }
}
