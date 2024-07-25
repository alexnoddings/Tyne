using Tyne.Searching;

namespace Tyne.Aerospace.Client.Features.Systems.Tables.Intro.Simple;

public class SimpleTableRequest : SearchQuery
{
    public string PlanetName { get; set; } = string.Empty;
}
