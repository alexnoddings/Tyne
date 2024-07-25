using Microsoft.AspNetCore.Components;

namespace Tyne.Aerospace.Client.Features.Systems.Filtering.Values.Select.Single.UpdateAvailableValues;

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
                _lastOrbitalBody = OrbitalBody;
                await SetValueAsync(null);
                await UpdateAvailableValuesAsync();
            }
            else
            {
                _lastOrbitalBody = OrbitalBody;
            }
        }
    }

    protected override async Task<List<IFilterSelectItem<string?>>> LoadAvailableValuesAsync()
    {
        // Simulates an API call
        await Task.Delay(1_000);

        List<IFilterSelectItem<string?>> orbitalTypes = OrbitalBody switch
        {
            OrbitalBody.Earth =>
            [
                FilterSelectItem.Create("Very low earth orbit", "VLEO"),
                FilterSelectItem.Create("Low earth orbit", "LEO"),
                FilterSelectItem.Create("Sun-synchronous orbit", "SSO"),
                FilterSelectItem.Create("Medium earth orbit", "MEO"),
                FilterSelectItem.Create("Geostationary transfer orbit", "GTO"),
                FilterSelectItem.Create("Geostationary orbit", "GEO"),
                FilterSelectItem.Create("Highly elliptical orbit", "HEO"),
            ],
            OrbitalBody.Moon =>
            [
                FilterSelectItem.Create("Low lunar orbit", "LLO"),
                FilterSelectItem.Create("Near-rectilinear halo orbit", "NRHO"),
                FilterSelectItem.Create("Distant retrograde orbit", "DRO")
            ],
            OrbitalBody.Mars =>
            [
                FilterSelectItem.Create("Low mars orbit", "LMO"),
                FilterSelectItem.Create("Areostationary orbit", "AEO"),
                FilterSelectItem.Create("Areosynchronous orbit", "ASO"),
            ],
            _ => throw new InvalidOperationException($"Unexpected value for {nameof(OrbitalBody)}: {OrbitalBody}.")
        };

        return orbitalTypes;
    }
}
