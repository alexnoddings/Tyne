using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace Tyne.Blazor;

public abstract partial class TyneFilteredColumnBase<TRequest, TResponse> :
    TyneColumn<TRequest, TResponse>,
    ITyneFilteredColumn<TRequest>,
    IDisposable
{
    [Inject]
    private ILoggerFactory LoggerFactory { get; init; } = null!;
    private ILogger? _logger;
    protected ILogger Logger =>
        _logger ??= LoggerFactory.CreateLogger(GetType());

    [CascadingParameter]
    protected ITyneTable<TRequest>? Table { get; init; }

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

    protected override Task OnInitializedAsync()
    {
        Logger.LogOnInitializedAsync();

        if (Table is null)
            throw new InvalidOperationException($"{nameof(TyneColumn<TRequest, TResponse>)} requires a cascading parameter of type {nameof(ITyneTable<TRequest>)}. Are you trying to create a column outside of a Tyne table?");

        _registration = Table.RegisterFilter(this);
        return Task.CompletedTask;
    }

    public abstract void ConfigureRequest(TRequest request);

    protected virtual async Task OnUpdatedAsync(CancellationToken cancellationToken = default)
    {
        if (Table is not null)
            await Table.ReloadServerDataAsync(cancellationToken).ConfigureAwait(true);
    }

    public abstract Task<bool> ClearValueAsync(CancellationToken cancellationToken = default);

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

internal static partial class TyneFilteredColumnBaseLogging
{
    [LoggerMessage(EventId = 101_002_001, Level = LogLevel.Trace, Message = $"TyneFilteredColumnBase`2.OnInitializedAsync()")]
    public static partial void LogOnInitializedAsync(this ILogger logger);

    [LoggerMessage(EventId = 101_002_002, Level = LogLevel.Trace, Message = $"TyneFilteredColumnBase`2.OnUpdatedAsync()")]
    public static partial void LogOnUpdatedAsync(this ILogger logger);
}
