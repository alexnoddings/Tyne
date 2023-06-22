using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Tyne.EntityFramework;

[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes", Justification = "Class is instantiated by Dependency Injection.")]
internal sealed class DbContextChangeAuditor : IDbContextChangeAuditor
{
    public const string IgnoreChangeAuditingAnnotationName = "Tyne:IgnoreChangeAuditing";
    public const string ParentNavigationForAuditingAnnotationName = "Tyne:ParentNavigationForAuditing";
    public const string ChildNavigationForAuditingAnnotationName = "Tyne:ChildNavigationForAuditing";

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

        var changeEvents = dbContext.ChangeTracker
            .Entries()
            .Where(entry => !entry.Metadata.HasFlagAnnotation(IgnoreChangeAuditingAnnotationName))
            .Select(entry => new
            {
                ChangeEvent = new DbContextChangeEvent
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
                        .Where(property => !property.Metadata.HasFlagAnnotation(IgnoreChangeAuditingAnnotationName))
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
                        .ToList(),
                    Parents = new()
                },
                ParentInfos =
                    entry.Navigations
                    .Where(navigationEntry =>
                        navigationEntry.Metadata.HasFlagAnnotation(ParentNavigationForAuditingAnnotationName)
                        || navigationEntry.Metadata.Inverse?.HasFlagAnnotation(ChildNavigationForAuditingAnnotationName) == true
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

        foreach (var changeEvent in changeEvents)
        {
            foreach (var parentInfo in changeEvent.ParentInfos)
            {
                var parentChangeEvent =
                    changeEvents
                    .Where(otherChangeEvent =>
                        otherChangeEvent.ChangeEvent.EntityId == parentInfo.Id
                        && otherChangeEvent.ChangeEvent.EntityType == parentInfo.Type
                    )
                    .Select(otherChangeEvent => otherChangeEvent.ChangeEvent)
                    .FirstOrDefault();

                if (parentChangeEvent is not null)
                    changeEvent.ChangeEvent.Parents.Add(parentChangeEvent);
            }
        }

        dbContext.AddRange(changeEvents.Select(changeEvent => changeEvent.ChangeEvent));
    }

    private static Guid? TryGetForeignKey(NavigationEntry navigationEntry)
    {
        if (navigationEntry.Metadata is not INavigation navigation)
            return null;

        if (navigation.ForeignKey.PrincipalKey.Properties.OfType<IPropertyBase>().FirstOrDefault() is not { } primaryKeyPropertyBase)
            return null;

        if (primaryKeyPropertyBase.PropertyInfo is not PropertyInfo primaryKeyPropertyInfo)
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
