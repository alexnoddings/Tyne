using Microsoft.EntityFrameworkCore;
using Tyne.EntityFramework;
using Tyne.Aerospace.Data.Entities;

namespace Tyne.Aerospace.Data;

public class AppDbContext : DbContext
{
    public DbSet<Part> Parts => Set<Part>();
    public DbSet<PartType> PartTypes => Set<PartType>();

    public DbSet<User> Users => Set<User>();

    private IDbContextModificationTracker DbContextModificationTracker { get; }

    public AppDbContext(DbContextOptions<AppDbContext> options, IDbContextModificationTracker dbContextModificationTracker) : base(options)
    {
        DbContextModificationTracker = dbContextModificationTracker;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        DbContextModificationTracker.TrackModifications(this);
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        await DbContextModificationTracker.TrackModificationsAsync(this, cancellationToken);
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
