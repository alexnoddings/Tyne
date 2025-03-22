using Microsoft.AspNetCore.Components;
using MudBlazor.Utilities;

namespace Tyne.Example.Client.Components;

public partial class TyneAerospaceIcon
{
    [Parameter]
    public MudColor MainColour { get; set; } = new("#207fba");

    [Parameter]
    public MudColor AccentColour { get; set; } = new("#b82828");
}
