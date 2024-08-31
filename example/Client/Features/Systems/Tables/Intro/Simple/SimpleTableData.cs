using Tyne.Searching;

namespace Tyne.Aerospace.Client.Features.Systems.Tables.Intro.Simple;

// Can't be static as then the SourceCode component can't use it as a generic type
public sealed class SimpleTableData
{
    private SimpleTableData() { }

    public static async Task<SearchResults<SimpleTableResponse>> GetDataAsync(SimpleTableRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Simulates an API call
        await Task.Delay(1000);

        var data = new List<SimpleTableResponse>
        {
            new("Mercury", 4_880, 57_910_000),
            new("Venus", 12_103, 108_210_000),
            new("Earth", 12_742, 149_598_023),
            new("Mars",  6_779, 227_939_366),
            new("Jupiter", 142_984, 778_479_000),
            new("Saturn", 116_464, 1_433_530_000),
            new("Uranus", 51_118, 2_870_972_000),
            new("Neptune", 49_528, 4_500_000_000),
        };

        var filtered =
            data.Where(planet =>
                    string.IsNullOrEmpty(request.PlanetName)
                    || planet.Name.Contains(request.PlanetName, StringComparison.OrdinalIgnoreCase)
                )
                .ToList();

        var ordered = filtered;
        if (request.OrderBy is { } orderBy)
        {
            ordered = [
                .. ordered
                .AsQueryable()
                .OrderByPropertyOrDefault(
                    propertyName: orderBy,
                    isDescending: request.OrderByDescending,
                    defaultKeySelector: planet => planet.DistanceFromSunKm,
                    defaultIsDescending: false
                )
            ];
        }

        var count = ordered.Count;
        var paginated = ordered.AsQueryable().Paginate(request);

        var searchResults = new SearchResults<SimpleTableResponse>(paginated, count);
        return searchResults;
    }
}
