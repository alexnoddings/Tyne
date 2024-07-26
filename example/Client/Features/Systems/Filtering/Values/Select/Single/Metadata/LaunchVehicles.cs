using System.Collections.ObjectModel;

namespace Tyne.Aerospace.Client.Features.Systems.Filtering.Values.Select.Single.Metadata;

public static class LaunchVehicles
{
    public static Collection<LaunchVehicle> Get() =>
    [
        new(Guid.Parse("C109D59A-0BA0-4B35-8D9F-2EC567AD3CE4"), "Falcon Heavy", 70, 97, 63_800),
        new(Guid.Parse("2E5761E1-8FA0-4FE9-92D0-31F5FA34E752"), "SLS", 111, 2_000, 131_541),
        new(Guid.Parse("549B41F2-E97C-4E46-A619-D007F3AA8CF6"), "Saturn V", 111, 1451, 141_136),
        new(Guid.Parse("B53F8EAF-0333-4BB7-8E51-9F1866877B12"), "Starship", 121, 100, 150_000),
    ];
}
