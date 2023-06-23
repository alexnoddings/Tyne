using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tyne.EntityFramework;

public class DbContextChangeEventRelation<TEvent, TProperty, TRelation>
    where TEvent : DbContextChangeEvent<TEvent, TProperty, TRelation>
    where TProperty : DbContextChangeEventProperty<TEvent, TProperty, TRelation>
    where TRelation : DbContextChangeEventRelation<TEvent, TProperty, TRelation>
{
    public Guid? ParentId { get; set; }
    public TEvent? Parent { get; set; }

    public Guid? ChildId { get; set; }
    public TEvent? Child { get; set; }
}

public class DbContextChangeEventRelationEntityTypeConfiguration<TEvent, TProperty, TRelation>
    : IEntityTypeConfiguration<TRelation>
    where TEvent : DbContextChangeEvent<TEvent, TProperty, TRelation>
    where TProperty : DbContextChangeEventProperty<TEvent, TProperty, TRelation>
    where TRelation : DbContextChangeEventRelation<TEvent, TProperty, TRelation>
{
    public virtual void Configure(EntityTypeBuilder<TRelation> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder
            .IgnoreChangeAuditing()
            .ToTable("_DbChangeRelations");
    }
}
