using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Tyne.EntityFramework;

[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes", Justification = "Class is instantiated by Dependency Injection.")]
internal sealed class DbContextChangeAuditor : IDbContextChangeAuditor
{
    private readonly ITyneUserService? _userService;

    public DbContextChangeAuditor(IEnumerable<ITyneUserService> userServices)
    {
        _userService = userServices.SingleOrDefault();
    }

    public void AuditChanges(DbContext dbContext)
    {
        AuditChangesCore(dbContext);
    }

    public Task AuditChangesAsync(DbContext dbContext, CancellationToken cancellationToken = default)
    {
        AuditChangesCore(dbContext);
        return Task.CompletedTask;
    }

    private void AuditChangesCore(DbContext dbContext)
    {
        var userId = _userService?.TryGetUserId();

        var changeEvents = new List<DbContextChangeEvent>();
        foreach (var entry in dbContext.ChangeTracker.Entries())
        {
            var entityType = entry.Entity?.GetType();
            if (entityType is null || entityType.GetCustomAttribute<SkipChangeAuditingAttribute>() is not null)
                continue;

            var change = new DbContextChangeEvent
            {
                DateTimeUtc = DateTime.UtcNow,
                UserId = userId,
                Action = entry.State.ToString(),
                EntityType = entityType.Name,
                EntityId =
                    entry.Properties
                    .Where(p => p.Metadata.IsPrimaryKey() && p.CurrentValue is Guid)
                    .Select(p => p.CurrentValue)
                    .OfType<Guid>()
                    .FirstOrDefault(),
                ActivityId = Activity.Current?.Id ?? string.Empty,
                Properties = entry.Properties
                .Where(property => entry.State is EntityState.Added or EntityState.Deleted || property.IsModified)
                .Select(property =>
                {
                    var oldValue =
                        entry.State is EntityState.Modified or EntityState.Deleted
                        ? JsonSerializer.Serialize(property.OriginalValue)
                        : null;

                    var newValue =
                        entry.State is EntityState.Added or EntityState.Modified
                        ? JsonSerializer.Serialize(property.CurrentValue)
                        : null;

                    return new DbContextChangeEventProperty
                    {
                        ColumnName = property.Metadata.Name,
                        OldValueJson = oldValue,
                        NewValueJson = newValue
                    };
                })
                .ToList()
            };

            changeEvents.Add(change);
        }

        dbContext.AddRange(changeEvents);
    }
}
