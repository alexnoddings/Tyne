namespace Tyne.Aerospace.Client.Features.Systems.Filtering.Values.Select.Single.Metadata;

public class LaunchVehicleSelectValue<TRequest> : TyneFilterSelectSingleValueBase<TRequest, Guid>
{
    protected override async Task<List<IFilterSelectItem<Guid>>> LoadAvailableValuesAsync()
    {
        // Simulates an API call
        await Task.Delay(1000);

        var launchVehicleSelectItems =
            LaunchVehicles
                .Get()
                // FilterSelectItem has a Create overload which takes metadata as a 3rd parameter
                // This returns a FilterSelectItem<T, TMetadata> which a controller can type-check for
                .Select(v => FilterSelectItem.Create(v.Id, v.Name, v))
                .ToList<IFilterSelectItem<Guid>>();

        return launchVehicleSelectItems;
    }
}
