using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tyne.EntityFramework;

public class DbContextChangeEventRelation
{
    public Guid? ParentId { get; set; }
    public DbContextChangeEvent? Parent { get; set; }

    public Guid? ChildId { get; set; }
    public DbContextChangeEvent? Child { get; set; }
}

public class DbContextChangeEventParentEntityTypeConfiguration : IEntityTypeConfiguration<DbContextChangeEventRelation>
{
    public void Configure(EntityTypeBuilder<DbContextChangeEventRelation> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder
            .IgnoreChangeAuditing()
            .ToTable("_DbChangeRelations");
    }
}
