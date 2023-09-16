using System.Runtime.Versioning;
using Microsoft.AspNetCore.Components;

namespace Tyne.Aerospace.Client.Infrastructure.Components;

[SupportedOSPlatform("browser")]
public partial class ActionByAt
{
    [Parameter, EditorRequired]
    public DateTime AtUtc { get; set; }

    [Parameter, EditorRequired]
    public string? ByName { get; set; }
}
