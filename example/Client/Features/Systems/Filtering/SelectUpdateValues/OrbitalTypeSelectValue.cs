

using Microsoft.AspNetCore.Components;

namespace Tyne.Aerospace.Client.Features.Systems.Filtering.SelectUpdateValues;

public class OrbitalTypeSelectValue<TRequest> : TyneFilterSelectSingleValueBase<TRequest, string>
{
    [Parameter]
    public OrbitalBody OrbitalBody { get; set; }
    // Used to track changes to OrbitalBody
    private OrbitalBody? _lastOrbitalBody;

    protected override async Task OnParametersSetAsync()
    {
        // If the orbital body parameter changes, reset the value and trigger an available value update
        if (_lastOrbitalBody != OrbitalBody)
        {
            // We only trigger an update if _lastOrbitalBody is not null
            // (otherwise this is the first set, so no need to reset)
            if (_lastOrbitalBody is not null)
            {
                await SetValueAsync(null);
                await UpdateAvailableValuesAsync();
            }

            _lastOrbitalBody = OrbitalBody;
        }
    }

    protected override Task<List<IFilterSelectItem<string?>>> LoadAvailableValuesAsync()
    {
        // This content could be loaded from an API instead
        List<IFilterSelectItem<string?>> orbitalTypes = OrbitalBody switch
        {
            OrbitalBody.Earth =>
            [
                FilterSelectItem.Create("LEO", "Low earth"),
                FilterSelectItem.Create("SSO", "Sun-synchronous"),
                FilterSelectItem.Create("GEO", "Geostationary"),
            ],
            OrbitalBody.Moon =>
            [
                FilterSelectItem.Create("LLO", "Low lunar"),
                FilterSelectItem.Create("NRHO", "Near-rectilinear halo"),
                FilterSelectItem.Create("DRO", "Distant retrograde")
            ],
            OrbitalBody.Mars =>
            [
                FilterSelectItem.Create("LMO", "Low mars"),
                FilterSelectItem.Create("AEO", "Areostationary"),
                FilterSelectItem.Create("ASO", "Areosynchronous"),
            ],
            _ => throw new InvalidOperationException($"Unexpected value for {nameof(OrbitalBody)}: {OrbitalBody}.")
        };

        return Task.FromResult(orbitalTypes);
    }
}
