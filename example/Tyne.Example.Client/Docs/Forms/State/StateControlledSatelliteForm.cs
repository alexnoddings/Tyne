using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tyne.Example.Client.Common.Data;

namespace Tyne.Example.Client.Docs.Forms.Spacecraft;

public class StateControlledSatelliteForm : TyneFormBase<Unit, Satellite, string>
{
    [Inject]
    private ISnackbar Snackbar { get; init; } = null!;

    [Parameter, EditorRequired]
    public Task<Result<Unit, string>> Operation { get; set; } = null!;

    protected override async Task<Result<Unit, string>> InitialiseAsync()
    {
        return await Operation;
    }

    protected override async Task<Result<Unit, string>> TrySaveAsync(Satellite model)
    {
        var result = await Operation;

        result.Apply(
            ok: _ => Snackbar.Add($"Satellite {model.Name} saved", Severity.Success, key: "satellite-saved", configure: o => o.SnackbarVariant = Variant.Outlined),
            error: error => Snackbar.Add($"Error saving satellite: {error}", Severity.Error, key: "satellite-errored")
        );

        return result;
    }
}
