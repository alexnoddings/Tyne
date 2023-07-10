using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor;

public abstract partial class TynePersistedColumnBase<TRequest, TResponse, TValue> :
    TyneFilteredColumnBase<TRequest, TResponse, TValue>,
    ITyneTablePersistedFilter<TValue>
{
    [Inject]
    protected NavigationManager NavigationManager { get; private init; } = null!;

    public abstract TyneFilterPersistKey PersistKey { get; }

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
        if (wasValueSet)
            await UpdatePersistedValueAsync().ConfigureAwait(true);

        return wasValueSet;
    }

    protected virtual async Task UpdatePersistedValueAsync() =>
        await TyneTablePersistedFilterHelpers.PersistValueAsync(this, NavigationManager).ConfigureAwait(true);
}
