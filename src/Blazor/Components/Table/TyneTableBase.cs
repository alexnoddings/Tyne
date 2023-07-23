using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tyne.Searching;

namespace Tyne.Blazor;

public abstract partial class TyneTableBase<TRequest, TResponse> :
    MudTable<TResponse>,
    ITyneTable<TRequest>
    where TRequest : ISearchQuery, new()
{
    private const string HeaderContentErrorMessage = $"HeaderContent should not be used on {nameof(TyneTableBase<TRequest, TResponse>)}s. Please use {nameof(TyneHeaderContent)} instead.";
    /// <summary>
    ///     Use <see cref="TyneHeaderContent"/> instead.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Blazor's default implementation for <see cref="ComponentBase.SetParametersAsync(ParameterView)"/> invokes ComponentProperties.SetProperties.
    ///         This implementation throws if multiple parameters have the same name, such as two HeaderContents.
    ///         MudBlazor does not mark the header content as virtual, so we can't override it.
    ///     </para>
    ///     <para>
    ///         One solution is to write custom logic for parameter setting, but ComponentProperties.cs is over 300 lines long,
    ///         so duplicating and modifying that would be a maintenance nightmare.
    ///     </para>
    ///     <para>
    ///         The other solution is to declare a new HeaderContent with an incompatible type, and to declare <see cref="TyneHeaderContent"/> instead.
    ///     </para>
    /// </remarks>
    [Obsolete(HeaderContentErrorMessage, error: true, DiagnosticId = "TY0001")]
    [SuppressMessage("Info Code Smell", "S1133: Deprecated code should be removed", Justification = "[Obsolete] is used to deter users from accidentally using this property.")]
    public new Unit HeaderContent
    {
        get => throw new InvalidOperationException(HeaderContentErrorMessage);
        set => throw new InvalidOperationException(HeaderContentErrorMessage);
    }

    [Parameter]
    public RenderFragment? TyneHeaderContent { get; set; }

    private const string ToolBarContentErrorMessage = $"ToolBarContent should not be used on {nameof(TyneTableBase<TRequest, TResponse>)}s. Please use {nameof(TyneToolBarContent)} instead.";
    /// <summary>
    ///     Use <see cref="TyneToolBarContent"/> instead.
    /// </summary>
    /// <remarks>
    ///     See <see cref="HeaderContent"/> for more info about this field.
    /// </remarks>
    [Obsolete(ToolBarContentErrorMessage, error: true, DiagnosticId = "TY0001")]
    [SuppressMessage("Info Code Smell", "S1133: Deprecated code should be removed", Justification = "[Obsolete] is used to deter users from accidentally using this property.")]
    public new Unit ToolBarContent
    {
        get => throw new InvalidOperationException(ToolBarContentErrorMessage);
        set => throw new InvalidOperationException(ToolBarContentErrorMessage);
    }

    [Parameter]
    public RenderFragment? TyneToolBarContent { get; set; }

    protected HashSet<ITyneTableRequestFilter<TRequest>> RegisteredFilters { get; } = new();

    protected TyneTableBase()
    {
        UserAttributes.Add("aria-role", "table");
        ServerData = LoadTableDataAsync;
    }

    protected override void OnParametersSet()
    {
        base.HeaderContent = BuildCustomContent(TyneHeaderContent);
        base.ToolBarContent = BuildCustomContent(TyneToolBarContent);
    }

    public Task ReloadServerDataAsync(CancellationToken cancellationToken = default) =>
        ReloadServerData();

    public IDisposable RegisterFilter(ITyneTableRequestFilter<TRequest> filter)
    {
        if (RegisteredFilters.Contains(filter))
            throw new ArgumentException("Filter is already registered.");

        RegisteredFilters.Add(filter);
        return new DisposableAction(() => RegisteredFilters.Remove(filter));
    }

    public async Task NotifySyncedFilterChangedAsync<TValue>(ITyneTableSyncedFilter<TValue> filterInstance, TValue? newValue, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filterInstance);

        var filterSyncKey = filterInstance.SyncKey;
        foreach (var otherFilter in RegisteredFilters.OfType<ITyneTableSyncedFilter<TValue>>())
        {
            if (otherFilter == filterInstance)
                continue;

            var otherFilterSyncKey = otherFilter.SyncKey;
            if (otherFilterSyncKey != filterSyncKey)
                continue;

            await otherFilter.SetValueAsync(newValue, true, cancellationToken).ConfigureAwait(true);
        }
    }

    private async Task<TableData<TResponse>> LoadTableDataAsync(TableState state)
    {
        ArgumentNullException.ThrowIfNull(state);

        var request = new TRequest
        {
            PageIndex = state.Page,
            PageSize = state.PageSize,
            OrderBy = state.SortLabel,
            OrderByDescending = state.SortDirection is SortDirection.Descending
        };

        foreach (var filter in RegisteredFilters)
            filter.ConfigureRequest(request);

        var searchResults = await LoadDataAsync(request).ConfigureAwait(true);

        return new TableData<TResponse>
        {
            TotalItems = searchResults.TotalCount,
            Items = searchResults
        };
    }

    protected abstract Task<SearchResults<TResponse>> LoadDataAsync(TRequest request);

    private RenderFragment? BuildCustomContent(RenderFragment? content)
    {
        if (content is null)
            return null;

        return builder =>
        {
            builder.OpenComponent<CascadingValue<ITyneTable<TRequest>>>(0);
            builder.AddAttribute(1, nameof(CascadingValue<object>.Value), this);
            builder.AddAttribute(2, nameof(CascadingValue<object>.IsFixed), true);
            builder.AddAttribute(3, nameof(CascadingValue<object>.ChildContent), content);
            builder.CloseComponent();
        };
    }
}
