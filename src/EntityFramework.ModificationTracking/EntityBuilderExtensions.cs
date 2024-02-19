using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tyne.EntityFramework;

namespace Microsoft.EntityFrameworkCore;

public static class EntityBuilderExtensions
{
    /// <summary>
    ///     Configures a one-to-many relationship between <typeparamref name="TEntity"/> and <typeparamref name="TUser"/> via <see cref="ICreatable{TUser}.CreatedBy"/>.
    /// </summary>
    /// <typeparam name="TEntity">The entity type being configured.</typeparam>
    /// <typeparam name="TUser">The type of user on <see cref="ICreatable{TUser}"/>.</typeparam>
    /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/>.</param>
    /// <returns>The <paramref name="builder"/> for chaining.</returns>
    public static EntityTypeBuilder<TEntity> IsCreatable<TEntity, TUser>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, ICreatable<TUser>
        where TUser : class
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder
            .HasOne(entity => entity.CreatedBy)
            .WithMany()
            .HasForeignKey(entity => entity.CreatedById)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        return builder;
    }

    /// <summary>
    ///     Configures a one-to-many relationship between <typeparamref name="TEntity"/> and <typeparamref name="TUser"/> via <see cref="IUpdatable{TUser}.LastUpdatedBy"/>.
    /// </summary>
    /// <typeparam name="TEntity">The entity type being configured.</typeparam>
    /// <typeparam name="TUser">The type of user on <see cref="IUpdatable{TUser}"/>.</typeparam>
    /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/>.</param>
    /// <returns>The <paramref name="builder"/> for chaining.</returns>
    public static EntityTypeBuilder<TEntity> IsUpdatable<TEntity, TUser>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IUpdatable<TUser>
        where TUser : class
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder
            .HasOne(entity => entity.LastUpdatedBy)
            .WithMany()
            .HasForeignKey(entity => entity.LastUpdatedById)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        return builder;
    }
}
