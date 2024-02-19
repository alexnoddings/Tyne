using System.Diagnostics.CodeAnalysis;

namespace Tyne.Aerospace.Client.Features.Systems.Filtering;

[SuppressMessage("Naming", "CA1720: Identifier contains type name", Justification = "This is an example. The properties are meaningless other than their types.")]
public class SimpleRequest
{
    public int Int { get; set; }
    public int? NullableInt { get; set; }
    public string String { get; set; } = string.Empty;
    public DateTime? DateMin { get; set; }
    public DateTime? DateMax { get; set; }
}
