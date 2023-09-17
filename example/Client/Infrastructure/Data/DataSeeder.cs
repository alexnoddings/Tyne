using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Tyne.Aerospace.Data.Entities;

namespace Tyne.Aerospace.Client.Infrastructure.Data;

public class DataSeeder
{
    private IAppDbContextFactory DbContextFactory { get; }
    private HttpClient HttpClient { get; }

    public DataSeeder(IAppDbContextFactory dbContextFactory, HttpClient httpClient)
    {
        DbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    private static JsonSerializerOptions PartsJsonDeserialisationOptions { get; } = new(JsonSerializerDefaults.Web)
    {
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        }
    };
    public async Task EnsureSeededAsync()
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        if (await dbContext.Parts.AnyAsync())
            return;

        var partsData = await HttpClient.GetFromJsonAsync<PartsData>("./data.json", PartsJsonDeserialisationOptions);
        if (partsData is null)
            return;

        dbContext.Users.AddRange(partsData.Users);
        dbContext.PartTypes.AddRange(partsData.PartTypes);
        dbContext.Parts.AddRange(partsData.Parts);

        await dbContext.SaveChangesAsync();
    }

    [SuppressMessage("Microsoft.Performance", "CA1812: Avoid uninstantiated internal classes", Justification = "Class is used during JSON deserialisation.")]
    private sealed class PartsData
    {
        public List<PartType> PartTypes { get; set; } = new();
        public List<Part> Parts { get; set; } = new();
        public List<User> Users { get; set; } = new();
    }
}
