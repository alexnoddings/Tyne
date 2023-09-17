using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tyne.Aerospace.Data.Entities;

public class Part : IEntity, ICreatable, IUpdatable
{
    public Guid Id { get; set; }

    public Guid TypeId { get; set; }
    public PartType Type { get; set; } = null!;

    public DateTime CreatedAtUtc { get; set; }
    public Guid? CreatedById { get; set; }
    public User? CreatedBy { get; set; }

    public DateTime LastUpdatedAtUtc { get; set; }
    public Guid? LastUpdatedById { get; set; }
    public User? LastUpdatedBy { get; set; }

    public string Name { get; set; } = string.Empty;
    public double PriceInPounds { get; set; }
    public PartSize Size { get; set; }
}

public class PartEntityTypeConfiguration : IEntityTypeConfiguration<Part>
{
    public void Configure(EntityTypeBuilder<Part> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder
            .IsEntity()
            .IsCreatable()
            .IsUpdatable();

        builder
            .HasIndex(part => part.Name);

        builder
            .Property(part => part.Name)
            .HasMaxLength(42);

        builder
            .HasOne(part => part.Type)
            .WithMany(partCategory => partCategory.Parts)
            .HasForeignKey(part => part.TypeId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
