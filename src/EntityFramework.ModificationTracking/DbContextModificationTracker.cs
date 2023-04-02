using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Tyne.EntityFramework;

[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes", Justification = "Class is instantiated by Dependency Injection.")]
internal sealed class DbContextModificationTracker : IDbContextModificationTracker
{
    private readonly ITyneUserService _userService;

    public DbContextModificationTracker(ITyneUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    public void TrackModifications(DbContext dbContext)
    {
        TrackModificationsCore(dbContext);
    }

    public Task TrackModificationsAsync(DbContext dbContext, CancellationToken cancellationToken = default)
    {
        TrackModificationsCore(dbContext);
        return Task.CompletedTask;
    }

    private void TrackModificationsCore(DbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        var utcNow = DateTime.UtcNow;
        var userId = _userService.UserId;

        foreach (var entityEntry in dbContext.ChangeTracker.Entries())
        {
            if (entityEntry.State is EntityState.Added)
            {
                if (entityEntry.Entity is ICreatable createdTrackedEntity)
                {
                    createdTrackedEntity.CreatedAtUtc = utcNow;
                    createdTrackedEntity.CreatedById = userId;
                }

                if (entityEntry.Entity is IUpdatable lastUpdatedTrackedEntity)
                {
                    lastUpdatedTrackedEntity.LastUpdatedAtUtc = utcNow;
                    lastUpdatedTrackedEntity.LastUpdatedById = userId;
                }
            }

            if (entityEntry.State is EntityState.Modified)
            {
                #pragma warning disable S1066
                // S1066: Collapsible "if" statements should be merged
                // This screws up the scope and conflicts with the 'lastUpdatedTrackedEntity' defined above
                if (entityEntry.Entity is IUpdatable lastUpdatedTrackedEntity)
                {
                    lastUpdatedTrackedEntity.LastUpdatedAtUtc = utcNow;
                    lastUpdatedTrackedEntity.LastUpdatedById = userId;
                }
                #pragma warning restore S1066
            }
        }
    }
}
