using Microsoft.EntityFrameworkCore;

namespace Tyne.Docs.Data;

public class DocsDbContext : DbContext
{
	public DbSet<Location> Locations { get; set; } = default!;
	public DbSet<Item> Items { get; set; } = default!;

	public DocsDbContext(DbContextOptions<DocsDbContext> options)
		: base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder
			.Entity<Location>()
			.HasKey(location => location.Id);

		builder
			.Entity<Item>()
			.HasKey(item => item.Id);

		builder
			.Entity<Item>()
			.HasOne(item => item.Location)
			.WithMany(location => location.Items)
			.HasForeignKey(item => item.LocationId)
			.OnDelete(DeleteBehavior.Cascade)
			.IsRequired();
	}
}
