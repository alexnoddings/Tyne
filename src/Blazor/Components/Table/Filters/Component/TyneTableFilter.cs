using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Tyne.Blazor;

public sealed class TyneTableFilter<TRequest, TValue> :
    TyneTableFilterBase<TRequest>,
    ITyneTablePersistedFilter<TValue>,
    ITyneTableSyncedFilter<TValue>
{
    [Parameter]
    public Expression<Func<TRequest, TValue?>>? For { get; set; }
    private Expression<Func<TRequest, TValue?>>? _previousFor;
    private PropertyInfo? _forPropertyInfo;

    [Parameter]
    public RenderFragment<ITyneTableFilterWrapper<TValue>> ChildContent { get; set; } =
        EmptyRenderFragment.For<ITyneTableFilterWrapper<TValue>>();

    [Parameter]
    public TValue? DefaultValue { get; set; }

    public TValue? Value { get; set; }

    private readonly TyneTableFilterWrapper<TValue> _wrapper;

    [Parameter]
    public string? PersistAs { get; set; }
    public TyneTableKey PersistKey =>
        TyneTableKey.From(PersistAs, _forPropertyInfo);

    [Inject]
    private NavigationManager NavigationManager { get; init; } = null!;

    [Parameter]
    public string? SyncAs { get; set; }
    public TyneTableKey SyncKey =>
        TyneTableKey.From(SyncAs, _forPropertyInfo);

    public TyneTableFilter()
    {
        _wrapper = new(
            () => Value,
            (newValue, cancellationToken) => SetValueAsync(newValue, isSilent: false, cancellationToken)
        );
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync().ConfigureAwait(true);

        // Needs to come before initialisation so that PersistKey will pick up the property info
        UpdateForPropertyInfo();

        var shouldSetDefaultValue = DefaultValue is not null;

        if (!PersistKey.IsEmpty)
        {
            var didSetPersistedValue = await TyneTablePersistedFilterHelpers.InitialisePersistedValueAsync(this, NavigationManager).ConfigureAwait(true);
            if (didSetPersistedValue && shouldSetDefaultValue)
                shouldSetDefaultValue = false;
        }

        if (shouldSetDefaultValue && !SyncKey.IsEmpty && Table is not null)
        {
            await SetValueAsync(DefaultValue, isSilent: true).ConfigureAwait(true);
            await Table.NotifySyncedFilterChangedAsync(this, Value).ConfigureAwait(true);
        }
        else if (Table is not null)
        {
            var syncedValue = await Table.TryGetSyncedFilterValue(this).ConfigureAwait(true);
            if (syncedValue is not null)
                await SetValueAsync(syncedValue, true).ConfigureAwait(true);
        }
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        UpdateForPropertyInfo();
    }

    private void UpdateForPropertyInfo() =>
        TyneTableFilterHelpers.UpdatePropertyInfo(For, ref _previousFor, ref _forPropertyInfo);

    public override void ConfigureRequest(TRequest request)
    {
        if (Value is not null)
            _forPropertyInfo?.SetValue(request, Value);
    }

    public async Task<bool> SetValueAsync(TValue? newValue, bool isSilent, CancellationToken cancellationToken = default)
    {
        if (EqualityComparer<TValue>.Default.Equals(newValue, Value))
            return false;

        Value = newValue;
        if (Table is not null && !isSilent)
        {
            if (!SyncKey.IsEmpty)
                await Table.NotifySyncedFilterChangedAsync(this, newValue, cancellationToken).ConfigureAwait(true);

            await Table.ReloadServerDataAsync(cancellationToken).ConfigureAwait(true);
        }

        if (!PersistKey.IsEmpty && !isSilent)
            await TyneTablePersistedFilterHelpers.PersistValueAsync(this, NavigationManager, cancellationToken).ConfigureAwait(true);

        return true;
    }

    public Task<bool> ClearValueAsync(CancellationToken cancellationToken = default) =>
        SetValueAsync(default, isSilent: false, cancellationToken);

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddContent(0, ChildContent, _wrapper);
    }
}
