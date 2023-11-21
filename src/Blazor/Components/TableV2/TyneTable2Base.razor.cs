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

    /// <summary>
    ///     Creates a <typeparamref name="TRequest"/> and configures it based on the arguments and the tables <see cref="IFilterContext{TRequest}"/>.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="orderBy">Which property, if any, to order by.</param>
    /// <param name="orderByDescending">
    ///     <see langword="true"/> to order <paramref name="orderBy"/> descending; otherwise, <see langword="false"/>.
    ///     This will be ignored if no <paramref name="orderBy"/> is specified.
    /// </param>
    /// <returns>
    ///     A <typeparamref name="TRequest"/> configured based on the arguments and the tables <see cref="IFilterContext{TRequest}"/>.
    ///     This may be <see langword="null"/> if a request could not be created, such as if the filter context has faulted.
    /// </returns>
    protected virtual async Task<TRequest?> CreateRequestAsync(int page, int pageSize, string? orderBy, bool orderByDescending)
    {
        if (_filterContext.IsFaulted)
            return default;

        var request = new TRequest
        {
            PageIndex = page,
            PageSize = pageSize,
            OrderBy = orderBy,
            OrderByDescending = orderByDescending
        };

        await _filterContext.WaitForInitialisedAsync().ConfigureAwait(true);
        await _filterContext.ConfigureRequestAsync(request).ConfigureAwait(true);

        return request;
    }

    /// <summary>
    ///     Creates a <typeparamref name="TRequest"/> and configures it based on based on <paramref name="state"/> and the tables <see cref="IFilterContext{TRequest}"/>.
    /// </summary>
    /// <param name="state">The current <see cref="TableState"/>.</param>
    /// <returns>
    ///     A <typeparamref name="TRequest"/> configured based on the <paramref name="state"/> and the tables <see cref="IFilterContext{TRequest}"/>.
    ///     This may be <see langword="null"/> if a request could not be created, such as if the filter context has faulted.
    /// </returns>
    /// <remarks>
    ///     This simply transforms <paramref name="state"/> into parameters for <see cref="CreateRequestAsync(int, int, string?, bool)"/>.
    ///     See that method for the actual request creation and configuration.
    /// </remarks>
    protected Task<TRequest?> CreateRequestAsync(TableState state)
    {
        ArgumentNullException.ThrowIfNull(state);

        string? orderBy;
        bool orderByDescending;

        if (state.SortDirection is SortDirection.Ascending)
        {
            orderBy = state.SortLabel;
            orderByDescending = false;
        }
        else if (state.SortDirection is SortDirection.Descending)
        {
            orderBy = state.SortLabel;
            orderByDescending = true;
        }
        else
        {
            orderBy = null;
            orderByDescending = false;
        }

        return CreateRequestAsync(state.Page, state.PageSize, orderBy, orderByDescending);
    }

    private async Task<TableData<TResponse>> LoadTableDataAsync(TableState state)
    {
        ArgumentNullException.ThrowIfNull(state);

        Logger.LogDebug("Loading table data.");

        var request = await CreateRequestAsync(state).ConfigureAwait(false);
        if (request is null)
        {
            return new TableData<TResponse>
            {
                TotalItems = 0,
                Items = Enumerable.Empty<TResponse>(),
            };
        }

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