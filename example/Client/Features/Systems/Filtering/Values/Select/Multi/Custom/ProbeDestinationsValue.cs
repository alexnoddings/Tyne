namespace Tyne.Aerospace.Client.Features.Systems.Filtering.Values.Select.Multi.Custom;

public class ProbeDestinationsValue<TRequest> : TyneFilterSelectMultiValueBase<TRequest, ProbeDestination>
{
    // This uses the same implementation of LoadAvailableValuesAsync as the single select.
    protected override async Task<List<IFilterSelectItem<ProbeDestination>>> LoadAvailableValuesAsync()
    {
        // Simulates an API call
        await Task.Delay(1_000);

        // Return a list of IFilterSelectItems.
        // These contain a value (the ProbeDestination) as well as how to render that value.
        // Here, that is simply with the enum name,
        // but the values here could be guids which are meaningless for a user.
        return [
            FilterSelectItem.Create(ProbeDestination.Mercury, "Mercury"),
            FilterSelectItem.Create(ProbeDestination.Venus, "Venus"),
            FilterSelectItem.Create(ProbeDestination.Earth, "Earth"),
            FilterSelectItem.Create(ProbeDestination.Mars, "Mars"),
            FilterSelectItem.Create(ProbeDestination.Jupiter, "Jupiter"),
            FilterSelectItem.Create(ProbeDestination.Saturn, "Saturn"),
            FilterSelectItem.Create(ProbeDestination.Uranus, "Uranus"),
            FilterSelectItem.Create(ProbeDestination.Neptune, "Neptune")
        ];
    }
}
