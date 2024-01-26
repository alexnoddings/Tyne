using Tyne.Aerospace.Data;
using Tyne.Aerospace.Data.Entities;

namespace Tyne.Aerospace.Client.Infrastructure.Data;

public static class DataSeeder
{
    private static readonly Guid _consumablePartType = Guid.Parse("c0fd962d-ef60-407f-8d08-48106d9d490b");
    private static readonly Guid _electronicsPartType = Guid.Parse("adc8449c-265b-4d2b-893a-70b988d3a9d3");
    private static readonly Guid _enginesPartType = Guid.Parse("0c93f835-91a1-4dd5-a12c-21f36b8d11b3");

    public static void EnsureSeeded(AppDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        var wasCreated = dbContext.Database.EnsureCreated();
        // If not created now, it has already been created
        if (!wasCreated)
            return;

        dbContext.Users.AddRange(
            User("7bd43de0-30e4-452b-85ec-54b3a20c520e", "Super user"),
            User("82f041b5-6acb-427b-98f8-8c459c773ce7", "Admin"),
            User("e05b0ee0-c6be-404f-ba6f-73593405865f", "User")
        );

        dbContext.PartTypes.AddRange(
            PartType(_consumablePartType, "Consumables"),
            PartType(_electronicsPartType, "Electronics"),
            PartType(_enginesPartType, "Engines")
        );

        dbContext.Parts.AddRange(
            Part("59286836-52ec-449b-8e87-75a630df644c", _consumablePartType, "Bolts", 0.03, PartSize.Small),
            Part("65b329b8-b6a0-4ed0-ab31-3a9759509ea8", _consumablePartType, "Nuts", 0.04, PartSize.Small),
            Part("8587335a-91e5-4be6-961e-b93797a96100", _electronicsPartType, "Radio", 2.73, PartSize.Medium),
            Part("3ceb68b7-0614-48d9-b182-82e331d6a2e1", _electronicsPartType, "Guidance computer", 847.99, PartSize.Large),
            Part("b1abf0fd-5095-40c0-822a-5c0da6aac6a8", _enginesPartType, "Pixel", 298.43, PartSize.Large),
            Part("3dac6f6e-c105-408c-b3b8-7e80a07f88db", _enginesPartType, "Shepherd", 945, PartSize.ExtraLarge)
        );

        dbContext.SaveChanges();
    }

    private static User User(string id, string name) =>
        new() { Id = Guid.Parse(id), Name = name };

    private static PartType PartType(Guid id, string name) =>
        new() { Id = id, Name = name };

    private static Part Part(string id, Guid typeId, string name, double price, PartSize size) =>
        new() { Id = Guid.Parse(id), TypeId = typeId, Name = name, PriceInPounds = price, Size = size };
}

