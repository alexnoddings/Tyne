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

        _ = entityTypeBuilder.HasAnnotation(DbContextChangeAuditor.IgnoreChangeAuditingAnnotationName, ignoreChangeAuditing);

        return entityTypeBuilder;
    }

    public static EntityTypeBuilder<TEntity> HasAuditParent<TEntity, TNavigation>(
        this EntityTypeBuilder<TEntity> entityTypeBuilder,
        Expression<Func<TEntity, TNavigation?>> navigationExpression
    )
        where TEntity : class
        where TNavigation : class
    {
        ArgumentNullException.ThrowIfNull(entityTypeBuilder);
        ArgumentNullException.ThrowIfNull(navigationExpression);

        _ = entityTypeBuilder
            .Navigation(navigationExpression)
            .HasAnnotation(DbContextChangeAuditor.ParentNavigationForAuditingAnnotationName, true);

        return entityTypeBuilder;
    }

    public static EntityTypeBuilder<TEntity> HasAuditChildren<TEntity, TNavigation>(
        this EntityTypeBuilder<TEntity> entityTypeBuilder,
        Expression<Func<TEntity, IEnumerable<TNavigation>?>> navigationExpression
    )
        where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(entityTypeBuilder);
        ArgumentNullException.ThrowIfNull(navigationExpression);

        _ = entityTypeBuilder
            .Navigation(navigationExpression)
            .HasAnnotation(DbContextChangeAuditor.ChildNavigationForAuditingAnnotationName, true);

        return entityTypeBuilder;
    }
}
