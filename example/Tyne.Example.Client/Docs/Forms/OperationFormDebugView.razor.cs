using Microsoft.AspNetCore.Components;

namespace Tyne.Example.Client.Docs.Forms.Spacecraft;

public partial class OperationFormDebugView : TyneFormComponentBase
{
    [Parameter]
    public Action<bool?> CompleteOperation { get; set; } = _ => { };
}
