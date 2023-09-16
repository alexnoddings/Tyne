using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tyne.Aerospace.Data.Entities;

public class PartType : IEntity, ICreatable, IUpdatable
{
    public Guid Id { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public Guid? CreatedById { get; set; }
    public User? CreatedBy { get; set; }

    public DateTime LastUpdatedAtUtc { get; set; }
    public Guid? LastUpdatedById { get; set; }
    public User? LastUpdatedBy { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<Part> Parts { get; set; } = null!;
}

public class PartCategoryEntityTypeConfiguration : IEntityTypeConfiguration<PartType>
{
    public void Configure(EntityTypeBuilder<PartType> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder
            .IsEntity()
            .IsCreatable()
            .IsUpdatable();

        builder
            .Property(partCategory => partCategory.Name)
            .HasMaxLength(42);
    }
}
