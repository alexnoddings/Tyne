using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tyne.EntityFramework;

namespace Microsoft.EntityFrameworkCore;

public static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder<TEntity> IgnoreChangeAuditing<TEntity>(
        this EntityTypeBuilder<TEntity> entityTypeBuilder,
        bool ignoreChangeAuditing = true
    )
        where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(entityTypeBuilder);

        entityTypeBuilder.HasAnnotation(DbContextChangeAuditor.IgnoreChangeAuditingAnnotationName, ignoreChangeAuditing);

        return entityTypeBuilder;
    }
}
