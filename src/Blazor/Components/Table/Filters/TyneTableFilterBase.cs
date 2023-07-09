using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor;

public abstract class TyneTableFilterBase<TRequest> :
    ComponentBase,
    ITyneTableRequestFilter<TRequest>,
    IDisposable
{
    [CascadingParameter]
    protected ITyneTable<TRequest>? Table { get; init; }

    private bool _isDisposed;
    private IDisposable? _registration;

    protected override Task OnInitializedAsync()
    {
        if (Table is null)
            throw new InvalidOperationException($"{nameof(TyneTableFilterBase<TRequest>)} requires a cascading parameter of type {nameof(ITyneTable<TRequest>)}. Are you trying to create a filter outside of a Tyne table?");

        _registration = Table.RegisterFilter(this);
        return Task.CompletedTask;
    }

    public abstract void ConfigureRequest(TRequest request);

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                // Unregister this filter from the table
                _registration?.Dispose();
                _registration = null;
            }

            _isDisposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
