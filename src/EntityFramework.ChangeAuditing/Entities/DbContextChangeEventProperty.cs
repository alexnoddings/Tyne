using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tyne.EntityFramework;

public class DbContextChangeEventProperty
{
    public Guid Id { get; set; }

    public Guid ChangeEventId { get; set; }
    public DbContextChangeEvent ChangeEvent { get; set; } = null!;

    public string ColumnName { get; set; } = string.Empty;
    public string? OldValueJson { get; set; } = string.Empty;
    public string? NewValueJson { get; set; } = string.Empty;
}

public class DbContextChangeEventPropertyEntityTypeConfiguration : IEntityTypeConfiguration<DbContextChangeEventProperty>
{
    public void Configure(EntityTypeBuilder<DbContextChangeEventProperty> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder
            .HasKey(changeEventProperty => changeEventProperty.Id);

        builder
            .IgnoreChangeAuditing()
            .ToTable("_DbChangesProperties");
    }
}
