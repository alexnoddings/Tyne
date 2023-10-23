using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MudBlazor;
using Tyne.Blazor.Filtering.Context;
using Tyne.Blazor.Filtering.Values;
using Tyne.Blazor.Persistence;
using Tyne.Searching;

namespace Tyne.Blazor.Tables;

// CA1848: Use the LoggerMessage delegates
//         Disabled while testing implementation
#pragma warning disable CA1848

/// <summary>
///     Base class for Tyne's table system.
/// </summary>
/// <typeparam name="TRequest">The type of request which loads the <typeparamref name="TResponse"/>s.</typeparam>
/// <typeparam name="TResponse">The type of data being loaded.</typeparam>
[CascadingTypeParameter(nameof(TRequest))]
[CascadingTypeParameter(nameof(TResponse))]
public abstract partial class TyneTable2Base<TRequest, TResponse>
{
    private const string ServerDataErrorMessage = $"ServerData should not be used on {nameof(TyneTable2Base<TRequest, TResponse>)}s as this is how Tyne overrides Mud's data loading.";
    /// <summary>
    ///     Do not use this property.
    /// </summary>
    /// <remarks>
    ///     This is a new property to prevent users from accidentally overriding it and breaking Tyne functionality.
    /// </remarks>
    [Obsolete(ServerDataErrorMessage, error: true, DiagnosticId = "TY0001")]
    [SuppressMessage("Info Code Smell", "S1133: Deprecated code should be removed", Justification = "[Obsolete] is used to stop users from using this property.")]
    public new Unit ServerData
    {
        get => throw new InvalidOperationException(ServerDataErrorMessage);
        set => throw new InvalidOperationException(ServerDataErrorMessage);
    }

    /// <summary>
    ///     The <see cref="IFilterValue{TValue}"/>s for filtering results.
    /// </summary>
    [Parameter]
    public RenderFragment? ValueFilters { get; set; }

    [Parameter]
    public bool PersistOrderBy { get; set; }

    [Parameter]
    public bool PersistPageNumber { get; set; }

    [Inject]
    private ILoggerFactory LoggerFactory { get; init; } = null!;

    [Inject]
    private IUrlPersistenceService PersistenceService { get; init; } = null!;

    private TyneFilterContext<TRequest> _filterContext = null!;

    /// <summary>
    ///     Constructs a new <see cref="TyneTable2Base{TRequest, TResponse}"/>.
    /// </summary>
    protected TyneTable2Base()
    {
        UserAttributes.Add("aria-role", "table");
        base.ServerData = LoadTableDataAsync;
    }

    /// <summary>
    ///     Initialises the <see cref="TyneTable2Base{TRequest, TResponse}"/>'s filtering context.
    /// </summary>
    protected override void OnInitialized()
    {
        Logger.LogDebug("Creating filter context.");
        var contextLogger = LoggerFactory.CreateLogger<TyneFilterContext<TRequest>>();
        _filterContext = new(contextLogger, PersistenceService, ReloadDataAsync);
    }

    protected override Task OnAfterRenderAsync(bool firstRender) =>
        firstRender
        ? InitialiseAndRenderAsync()
        : base.OnAfterRenderAsync(false);

    // Runs first-time initialisation after first render
    private async Task InitialiseAndRenderAsync()
    {
        Logger.LogDebug("Initialising table.");

        Loading = true;
        StateHasChanged();

        try
        {
            await _filterContext.InitialiseAsync().ConfigureAwait(false);
        }
        catch
        {
            Loading = false;
            StateHasChanged();
            throw;
        }

        Loading = false;
        await base.OnAfterRenderAsync(true).ConfigureAwait(false);
    }

    /// <summary>
    ///     Causes the data in the table to be reloaded.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the reload.</returns>
    public Task ReloadDataAsync()
    {
        Logger.LogDebug("Reloading server data.");
        return ReloadServerData();
    }

    private async Task<TableData<TResponse>> LoadTableDataAsync(TableState state)
    {
        ArgumentNullException.ThrowIfNull(state);

        Logger.LogDebug("Loading table data.");

        if (_filterContext.IsFaulted)
        {
            return new TableData<TResponse>
            {
                TotalItems = 0,
                Items = Enumerable.Empty<TResponse>(),
            };
        }

        var request = new TRequest
        {
            PageIndex = state.Page,
            PageSize = state.PageSize
        };

        if (state.SortDirection is SortDirection.Ascending)
        {
            request.OrderBy = state.SortLabel;
            request.OrderByDescending = false;
        }
        else if (state.SortDirection is SortDirection.Descending)
        {
            request.OrderBy = state.SortLabel;
            request.OrderByDescending = true;
        }

        await _filterContext.WaitForInitialisedAsync().ConfigureAwait(true);
        await _filterContext.ConfigureRequestAsync(request).ConfigureAwait(true);

        var searchResults = await LoadDataAsync(request).ConfigureAwait(true);

        return new TableData<TResponse>
        {
            TotalItems = searchResults.TotalCount,
            Items = searchResults
        };
    }

    protected abstract Task<SearchResults<TResponse>> LoadDataAsync(TRequest request);

    protected virtual void Dispose(bool disposing)
    {
        _filterContext.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
