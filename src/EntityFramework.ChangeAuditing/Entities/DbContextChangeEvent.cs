using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tyne.EntityFramework;

[SuppressMessage("Design", "CA1002: Do not expose generic lists", Justification = "This behaviour is sensible for EF models.")]
[SuppressMessage("Usage", "CA2227: Collection properties should be read only", Justification = "This behaviour is sensible for EF models.")]
public class DbContextChangeEvent<TEvent, TProperty, TRelation>
    where TEvent : DbContextChangeEvent<TEvent, TProperty, TRelation>
    where TProperty : DbContextChangeEventProperty<TEvent, TProperty, TRelation>
    where TRelation : DbContextChangeEventRelation<TEvent, TProperty, TRelation>
{
    public Guid Id { get; set; }

    public DateTime DateTimeUtc { get; set; }
    public Guid? UserId { get; set; }

    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public string? ActivityId { get; set; }

    public List<TProperty> Properties { get; set; } = null!;

    public List<TEvent> Parents { get; set; } = null!;
    public List<TRelation> ParentRelations { get; set; } = null!;

    public List<TEvent> Children { get; set; } = null!;
    public List<TRelation> ChildRelations { get; set; } = null!;
}

public abstract class DbContextChangeEventEntityTypeConfiguration<TEvent, TProperty, TRelation>
    : IEntityTypeConfiguration<TEvent>
    where TEvent : DbContextChangeEvent<TEvent, TProperty, TRelation>
    where TProperty : DbContextChangeEventProperty<TEvent, TProperty, TRelation>
    where TRelation : DbContextChangeEventRelation<TEvent, TProperty, TRelation>
{
    public virtual void Configure(EntityTypeBuilder<TEvent> builder)
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
            .HasMany(changeEvent => changeEvent.Children)
            .WithMany(changeEvent => changeEvent.Parents)
            .UsingEntity<TRelation>(
                j => j
                    .HasOne(changeEvent => changeEvent.Child)
                    .WithMany(relation => relation.ChildRelations)
                    .HasForeignKey(relation => relation.ChildId),
                j => j
                    .HasOne(changeEvent => changeEvent.Parent)
                    .WithMany(relation => relation.ParentRelations)
                    .HasForeignKey(relation => relation.ParentId),
                j => j
                    .HasKey(relation => new { relation.ParentId, relation.ChildId })
            );

        builder
            .IgnoreChangeAuditing()
            .ToTable("_DbChanges");
    }
}
