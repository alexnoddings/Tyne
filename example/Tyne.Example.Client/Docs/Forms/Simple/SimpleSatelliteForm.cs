using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tyne.Example.Client.Common.Data;

namespace Tyne.Example.Client.Docs.Forms.Spacecraft;

public class SimpleSatelliteForm : TyneFormBase<Unit, Satellite, string>
{
    [Inject]
    private ISnackbar Snackbar { get; init; } = null!;

    protected override async Task<Result<Unit, string>> InitialiseAsync()
    {
        // Simulate initialising some remote data from the server
        await Task.Delay(1_000);
        return unit;
    }

    protected override async Task<Result<Unit, string>> TrySaveAsync(Satellite model)
    {
        // Simulate saving the model to the server
        await Task.Delay(1_000);
        Snackbar.Add($"Satellite {model?.Name} saved", Severity.Success, key: "satellite-saved", configure: o =>
        {
            o.SnackbarVariant = Variant.Outlined;
        });
        return unit;
    }
}
