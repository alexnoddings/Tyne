using Microsoft.AspNetCore.Components;
using MudBlazor.Utilities;

namespace Tyne.Aerospace.Client.Infrastructure;

public partial class TyneAerospaceIcon
{
    [Parameter]
    public MudColor MainColour { get; set; } = new MudColor("#207fba");

    [Parameter]
    public MudColor AccentColour { get; set; } = new MudColor("#b82828");
}
