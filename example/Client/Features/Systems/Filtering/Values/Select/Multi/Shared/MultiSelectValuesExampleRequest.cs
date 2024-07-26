namespace Tyne.Aerospace.Client.Features.Systems.Filtering.Values.Select.Multi;

// CA2227: Collection properties should be read only
//         Property must be mutable for Tyne to set it
#pragma warning disable CA2227
public class MultiSelectValuesExampleRequest
{
    public HashSet<ProbeDestination> ProbeDestinations { get; set; } = [];
}
#pragma warning restore CA2227
