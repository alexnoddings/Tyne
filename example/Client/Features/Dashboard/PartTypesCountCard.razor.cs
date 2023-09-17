using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Tyne.Aerospace.Client.Features.Dashboard;

public sealed partial class PartTypesCountCard
{
    [Inject]
    private IAppDbContextFactory AppDbContextFactory { get; init; } = null!;
    private int? Count { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await using var dbContext = await AppDbContextFactory.CreateDbContextAsync();
        Count = await dbContext.PartTypes.CountAsync();
    }
}
