using Tyne.EntityFramework;

namespace Microsoft.EntityFrameworkCore;

public static class ModelBuilderApplyConfigurationExtensions
{
    public static ModelBuilder ApplyAuditingConfigurations(this ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        return modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbContextChangeEventEntityTypeConfiguration).Assembly);
    }
}
