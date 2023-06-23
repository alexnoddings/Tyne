using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tyne.EntityFramework;

public class DbContextChangeEventProperty<TEvent, TProperty, TRelation>
    where TEvent : DbContextChangeEvent<TEvent, TProperty, TRelation>
    where TProperty : DbContextChangeEventProperty<TEvent, TProperty, TRelation>
    where TRelation : DbContextChangeEventRelation<TEvent, TProperty, TRelation>
{
    public Guid Id { get; set; }

    public Guid ChangeEventId { get; set; }
    public TEvent ChangeEvent { get; set; } = null!;

    public string ColumnName { get; set; } = string.Empty;
    public string ColumnType { get; set; } = string.Empty;
    public string? OldValueJson { get; set; } = string.Empty;
    public string? NewValueJson { get; set; } = string.Empty;
}

public abstract class DbContextChangeEventPropertyEntityTypeConfiguration<TEvent, TProperty, TRelation>
    : IEntityTypeConfiguration<TProperty>
    where TEvent : DbContextChangeEvent<TEvent, TProperty, TRelation>
    where TProperty : DbContextChangeEventProperty<TEvent, TProperty, TRelation>
    where TRelation : DbContextChangeEventRelation<TEvent, TProperty, TRelation>
{
    public virtual void Configure(EntityTypeBuilder<TProperty> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder
            .HasKey(changeEventProperty => changeEventProperty.Id);

        builder
            .IgnoreChangeAuditing()
            .ToTable("_DbChangesProperties");
    }
}
