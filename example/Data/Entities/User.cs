using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Tyne.Aerospace.Data.Entities;

public class User : IEntity, ICreatable, IUpdatable
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }
    public Guid? CreatedById { get; set; }
    public User? CreatedBy { get; set; }

    public DateTime LastUpdatedAtUtc { get; set; }
    public Guid? LastUpdatedById { get; set; }
    public User? LastUpdatedBy { get; set; }
}

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder
            .IsEntity()
            .IsCreatable()
            .IsUpdatable();

        builder
            .Property(user => user.Name)
            .HasMaxLength(42);
    }
}
