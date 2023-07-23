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

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync().ConfigureAwait(true);

        if (!PersistKey.IsEmpty)
            await InitialisePersistedValueAsync().ConfigureAwait(true);
    }

    protected virtual async Task InitialisePersistedValueAsync() =>
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
