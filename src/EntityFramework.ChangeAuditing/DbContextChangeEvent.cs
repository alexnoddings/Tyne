using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tyne.EntityFramework;

[SkipChangeAuditing]
public class DbContextChangeEvent
{
    public Guid Id { get; set; }

    public DateTime DateTimeUtc { get; set; }
    public Guid? UserId { get; set; }

    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public string? ActivityId { get; set; }

    [SuppressMessage("Usage", "CA2227: Collection properties should be read only", Justification = "Behaviour is fine for EF models.")]
    [SuppressMessage("Design", "CA1002: Do not expose generic lists", Justification = "Behaviour is fine for EF models.")]
    public List<DbContextChangeEventProperty> Properties { get; set; } = null!;
}

public class DbContextChangeEventEntityTypeConfiguration : IEntityTypeConfiguration<DbContextChangeEvent>
{
    public void Configure(EntityTypeBuilder<DbContextChangeEvent> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasKey(changeEvent => changeEvent.Id);

        builder.HasIndex(changeEvent => changeEvent.Action);
        builder.HasIndex(changeEvent => changeEvent.EntityType);

        builder
            .HasMany(changeEvent => changeEvent.Properties)
            .WithOne(changeEventProperty => changeEventProperty.ChangeEvent)
            .HasForeignKey(changeEventProperty => changeEventProperty.ChangeEventId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .ToTable("_DbChanges");
    }
}
