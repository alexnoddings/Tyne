using MudBlazor;

namespace Tyne.Example.Client.Docs.Tables.Intro.Simple;

// Can't be static as then the SourceCode component can't use it as a generic type
public sealed class SimpleTableData
{
    private SimpleTableData() { }

    public static async Task<TableData<SimpleTableResponse>> GetDataAsync(SimpleTableRequest request)
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
        }.AsQueryable();

        data = data.Where(planet =>
                    string.IsNullOrEmpty(request.PlanetName)
                    || planet.Name.Contains(request.PlanetName, StringComparison.OrdinalIgnoreCase)
                );

        if (request.OrderBy is { } orderBy)
        {
            if (request.OrderByDescending)
            {
                data = orderBy switch
                {
                    nameof(SimpleTableResponse.Name) => data.OrderBy(x => x.Name),
                    nameof(SimpleTableResponse.DiameterKm) => data.OrderBy(x => x.DiameterKm),
                    nameof(SimpleTableResponse.DistanceFromSunKm) => data.OrderBy(x => x.DistanceFromSunKm),
                    _ => data
                };
            }
            else
            {
                data = orderBy switch
                {
                    nameof(SimpleTableResponse.Name) => data.OrderByDescending(x => x.Name),
                    nameof(SimpleTableResponse.DiameterKm) => data.OrderByDescending(x => x.DiameterKm),
                    nameof(SimpleTableResponse.DistanceFromSunKm) => data.OrderByDescending(x => x.DistanceFromSunKm),
                    _ => data
                };
            }
        }

        var count = data.Count();
        var paginated = data
            .Skip(request.Page * request.PageSize)
            .Take(request.PageSize);

        var searchResults = new TableData<SimpleTableResponse>
        {
            Items = paginated,
            TotalItems = count
        };
        return searchResults;
    }
}
