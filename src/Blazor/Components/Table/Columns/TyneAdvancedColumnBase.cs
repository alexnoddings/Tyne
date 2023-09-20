using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor;

/// <summary>
///     A base class for advanced filtered columns.
///     This currently includes
///     <see cref="ITyneTablePersistedFilter{TValue}"/> and
///     <see cref="ITyneTableSyncedFilter{TValue}"/> logic.
/// </summary>
/// <remarks>
///     To only not utilise <see cref="ITyneTablePersistedFilter"/> or <see cref="ITyneTableSyncedFilter"/>,
///     simply set their respective keys to <c>Key => <see cref="TyneTableKey.Empty"/></c>.
/// </remarks>
public abstract partial class TyneAdvancedColumnBase<TRequest, TResponse, TValue> :
    TyneFilteredColumnBase<TRequest, TResponse, TValue>,
    ITyneTablePersistedFilter<TValue>,
    ITyneTableSyncedFilter<TValue>
{
    [Inject]
    protected NavigationManager NavigationManager { get; private init; } = null!;

    public abstract TyneTableKey PersistKey { get; }

    public abstract TyneTableKey SyncKey { get; }

    [Parameter]
    public TValue? DefaultValue { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync().ConfigureAwait(true);

        var shouldSetDefaultValue = DefaultValue is not null;

        if (!PersistKey.IsEmpty)
        {
            var didSetPersistedValue = await InitialisePersistedValueAsync().ConfigureAwait(true);
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

    protected virtual async Task<bool> InitialisePersistedValueAsync() =>
        await TyneTablePersistedFilterHelpers.InitialisePersistedValueAsync(this, NavigationManager).ConfigureAwait(true);

    public override async Task<bool> SetValueAsync(TValue? newValue, bool isSilent, CancellationToken cancellationToken = default)
    {
        var wasValueSet = await base.SetValueAsync(newValue, isSilent, cancellationToken).ConfigureAwait(true);
        if (wasValueSet && !isSilent)
            await UpdatePersistedValueAsync().ConfigureAwait(true);

        return wasValueSet;
    }

    protected override async Task OnUpdatedAsync(CancellationToken cancellationToken = default)
    {
        var syncKey = SyncKey;
        if (!syncKey.IsEmpty && Table is not null)
            await Table.NotifySyncedFilterChangedAsync(this, Value, cancellationToken).ConfigureAwait(true);

        await base.OnUpdatedAsync(cancellationToken).ConfigureAwait(true);
    }

    protected virtual async Task UpdatePersistedValueAsync() =>
        await TyneTablePersistedFilterHelpers.PersistValueAsync(this, NavigationManager).ConfigureAwait(true);
}
