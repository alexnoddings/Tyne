using System.Diagnostics.CodeAnalysis;

namespace Tyne.Aerospace.Client.Infrastructure.Layouts;

[SuppressMessage("Microsoft.Performance", "CA1812: Avoid uninstantiated internal classes.", Justification = "Instantiated by DI.")]
internal sealed class BuildTag
{
    public string? Tag { get; set; }
}
