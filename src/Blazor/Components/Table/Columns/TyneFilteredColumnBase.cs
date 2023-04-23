using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Tyne.Blazor;

public abstract partial class TyneFilteredColumnBase<TRequest, TResponse> :
    TyneColumn<TRequest, TResponse>,
    ITyneFilteredColumn<TRequest>,
    IDisposable
{
    [CascadingParameter]
    protected ITyneTable<TRequest, TResponse>? Table { get; init; }

    public bool IsFilterVisible { get; set; }
    public abstract bool IsFilterActive { get; }
    public string Icon
    {
        get
        {
            if (IsFilterActive)
                return Icons.Material.Filled.SavedSearch;

            if (IsFilterVisible)
                return Icons.Material.Filled.SearchOff;

            return Icons.Material.Filled.Search;
        }
    }

    public Color IconColour =>
        IsFilterActive
        ? Color.Primary
        : Color.Default;

    public string? OrderByName => OrderByProperty?.Name;

    private bool _isDisposed;
    private IDisposable? _registration;

    protected override void OnInitialized()
    {
        if (Table is null)
            throw new InvalidOperationException($"{nameof(TyneColumn<TRequest, TResponse>)} requires a cascading parameter of type {nameof(ITyneTable<TRequest, TResponse>)}. Are you trying to create a column outside of a Tyne table?");

        _registration = Table.RegisterColumn(this);
    }

    public abstract void ConfigureRequest(TRequest request);

    protected async Task OnUpdatedAsync()
    {
        if (Table is not null)
            await Table.ReloadServerDataAsync().ConfigureAwait(true);
    }

    public abstract Task ClearInputAsync();

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                // Unregister this column from the table
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
