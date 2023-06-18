using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tyne.EntityFramework;

namespace Microsoft.EntityFrameworkCore;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<TProperty> IgnoreChangeAuditing<TProperty>(this PropertyBuilder<TProperty> propertyBuilder, bool ignoreChangeAuditing = true)
    {
        ArgumentNullException.ThrowIfNull(propertyBuilder);

        propertyBuilder.Metadata.SetAnnotation(DbContextChangeAuditor.IgnoreChangeAuditingAnnotationName, ignoreChangeAuditing);

        return propertyBuilder;
    }
}
