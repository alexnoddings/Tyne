namespace Tyne.Aerospace.Client.Features.Systems.Filtering.Values.Search;

public class GalaxySearchFilterValue<TRequest>
    : TyneSearchFilterValue<TRequest, GalaxyInfo, string>
{
    // The controller is configured to display a prompt if
    // there are more results if >10 are shown (MaxItems="10")
    private const int MaxResults = 11;

    public override async Task<List<GalaxyInfo>> SearchAsync(string? search, CancellationToken cancellationToken = default)
    {
        // Simulates an API call
        await Task.Delay(Random.Shared.Next(250, 750), cancellationToken);

        var galaxies = Galaxies.Instance.OrderBy(galaxy => galaxy.Name);

        // Return all galaxies if no search term
        if (string.IsNullOrEmpty(search))
            return [.. galaxies.Take(MaxResults)];

        // Otherwise, return all galaxies whose names contain the search term
        return galaxies
            .Where(galaxy => galaxy.Name.Contains(search, StringComparison.OrdinalIgnoreCase))
            .Take(MaxResults)
            .ToList();
    }

    // Transforms a TSearchValue (GalaxyInfo) used when searching
    // into a TFilterValue (string) used on the TRequest
    protected override string? GetFilterValueFrom(GalaxyInfo searchValue) =>
        searchValue?.Name;
}
