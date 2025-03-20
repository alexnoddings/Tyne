using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor;

public abstract class TyneFormComponentBase : ComponentBase, IDisposable
{
    [CascadingParameter]
    protected ITyneForm Form { get; private set; } = null!;

    private IDisposable? _stateChangedHook;

    protected override bool ShouldRender() =>
        // Prevents rendering if no Form was provided
        Form is not null && base.ShouldRender();

    protected override void OnParametersSet()
    {
        if (Form is null)
        {
            throw new InvalidOperationException(
                $"{GetType().Name} requires a cascading parameter of type {nameof(ITyneForm)}. " +
                $"Are you trying to render this component outside of a form?"
            );
        }
    }

    protected override void OnInitialized()
    {
        _stateChangedHook = Form?.WatchForStateChanges(OnFormStateChangedAsync);
    }

    protected virtual Task OnFormStateChangedAsync() => InvokeAsync(StateHasChanged);

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _stateChangedHook?.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
