using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Tyne.EntityFramework;

[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes", Justification = "Class is instantiated by Dependency Injection.")]
public class DbContextChangeAuditor<TEvent, TProperty, TRelation> : IDbContextChangeAuditor
    where TEvent : DbContextChangeEvent<TEvent, TProperty, TRelation>, new()
    where TProperty : DbContextChangeEventProperty<TEvent, TProperty, TRelation>, new()
    where TRelation : DbContextChangeEventRelation<TEvent, TProperty, TRelation>, new()
{
    private readonly ITyneUserService? _userService;

    public DbContextChangeAuditor(IEnumerable<ITyneUserService> userServices)
    {
        _userService = userServices.SingleOrDefault();
    }

    public void AuditChanges(DbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        var changeEventEntries = AuditChangesCore(dbContext);
        var changeEvents = OnAuditing(dbContext, changeEventEntries);
        dbContext.AddRange(changeEvents);
    }

    protected virtual IEnumerable<TEvent> OnAuditing(DbContext dbContext, ChangeEventEntries changeEventEntries) =>
        changeEventEntries.Select(changeEventEntry => changeEventEntry.Event);

    public async Task AuditChangesAsync(DbContext dbContext, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        var changeEventEntries = AuditChangesCore(dbContext);
        var changeEvents = await OnAuditingAsync(dbContext, changeEventEntries).ConfigureAwait(false);
        dbContext.AddRange(changeEvents);
    }

    protected virtual ValueTask<IEnumerable<TEvent>> OnAuditingAsync(DbContext dbContext, ChangeEventEntries changeEventEntries) =>
        ValueTask.FromResult(changeEventEntries.Select(changeEventEntry => changeEventEntry.Event));

    protected sealed record ChangeEventEntry(TEvent Event, EntityEntry Entry);
    protected class ChangeEventEntries : List<ChangeEventEntry>
    {
        public ChangeEventEntries(IEnumerable<ChangeEventEntry> collection) : base(collection)
        {
        }
    }

    private ChangeEventEntries AuditChangesCore(DbContext dbContext)
    {
        var userId = _userService?.TryGetUserId();

        var changeEventInfos = dbContext.ChangeTracker
            .Entries()
            .Where(entry => !entry.Metadata.HasFlagAnnotation(DbContextChangeAuditor.IgnoreChangeAuditingAnnotationName))
            .Select(entry => new
            {
                EntityEntry = entry,
                ChangeEvent = new TEvent
                {
                    Id = Guid.NewGuid(),
                    DateTimeUtc = DateTime.UtcNow,
                    UserId = userId,
                    Action = entry.State.ToString(),
                    EntityType = entry.Entity.GetType().Name,
                    EntityId =
                        entry.Properties
                        .Where(p => p.Metadata.IsPrimaryKey() && p.CurrentValue is Guid)
                        .Select(p => p.CurrentValue)
                        .OfType<Guid>()
                        .FirstOrDefault(),
                    ActivityId = Activity.Current?.Id ?? string.Empty,
                    Properties =
                        entry.Properties
                        .Where(property => entry.State is EntityState.Added or EntityState.Deleted || property.IsModified)
                        .Where(property => !property.Metadata.HasFlagAnnotation(DbContextChangeAuditor.IgnoreChangeAuditingAnnotationName))
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

                            return new TProperty
                            {
                                ColumnName = property.Metadata.Name,
                                ColumnType = DbContextChangeAuditorTypeNameFormatter.FormatTypeName(property.Metadata.ClrType),
                                OldValueJson = oldValue,
                                NewValueJson = newValue
                            };
                        })
                        .ToList(),
                    Parents = []
                },
                ParentInfos =
                    entry.Navigations
                    .Where(navigationEntry =>
                        navigationEntry.Metadata.HasFlagAnnotation(DbContextChangeAuditor.ParentNavigationForAuditingAnnotationName)
                        || navigationEntry.Metadata.Inverse?.HasFlagAnnotation(DbContextChangeAuditor.ChildNavigationForAuditingAnnotationName) == true
                    )
                    .Where(navigationEntry => !navigationEntry.Metadata.IsCollection)
                    .Select(navigationEntry => new
                    {
                        Type = navigationEntry.CurrentValue?.GetType().Name ?? string.Empty,
                        Id = TryGetForeignKey(navigationEntry)
                    })
                    .ToList(),
            })
            .ToList();

        foreach (var changeEventInfo in changeEventInfos)
        {
            foreach (var parentInfo in changeEventInfo.ParentInfos)
            {
                var parentChangeEvent =
                    changeEventInfos
                    .Where(otherChangeEvent =>
                        otherChangeEvent.ChangeEvent.EntityId == parentInfo.Id
                        && otherChangeEvent.ChangeEvent.EntityType == parentInfo.Type
                    )
                    .Select(otherChangeEvent => otherChangeEvent.ChangeEvent)
                    .FirstOrDefault();

                if (parentChangeEvent is not null)
                    changeEventInfo.ChangeEvent.Parents.Add(parentChangeEvent);
            }
        }

        var changeEventEntries = changeEventInfos
            .Select(changeEventInfo => new ChangeEventEntry(changeEventInfo.ChangeEvent, changeEventInfo.EntityEntry))
            .Where(pair => pair.Event.Action is not nameof(EntityState.Detached) and not nameof(EntityState.Unchanged))
            .ToList();

        return new ChangeEventEntries(changeEventEntries);
    }

    private static Guid? TryGetForeignKey(NavigationEntry navigationEntry)
    {
        if (navigationEntry.Metadata is not INavigation navigation)
            return null;

        var primaryKeyPropertyBase =
            navigation
                .ForeignKey
                .PrincipalKey
                .Properties
                .OfType<IPropertyBase>()
                .FirstOrDefault();

        if (primaryKeyPropertyBase?.PropertyInfo is not { } primaryKeyPropertyInfo)
            return null;

        if (primaryKeyPropertyInfo.PropertyType != typeof(Guid))
            return null;

        try
        {
            if (primaryKeyPropertyInfo.GetValue(navigationEntry.CurrentValue) is not Guid foreignKeyValue)
                return null;

            return foreignKeyValue;
        }
        catch (TargetException)
        {
            // May be thrown if the navigation isn't a type we expect (e.g. a collection<T> instead of T)
            return null;
        }
    }
}
