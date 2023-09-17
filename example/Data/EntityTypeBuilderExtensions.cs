using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tyne.Aerospace.Data.Entities;

namespace Tyne.Aerospace.Data;

internal static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder<T> IsEntity<T>(this EntityTypeBuilder<T> builder) where T : class, IEntity
    {
        builder.HasKey(entity => entity.Id);

        return builder;
    }

    public static EntityTypeBuilder<T> IsCreatable<T>(this EntityTypeBuilder<T> builder) where T : class, ICreatable =>
        builder.IsCreatable<T, User>();

    public static EntityTypeBuilder<T> IsUpdatable<T>(this EntityTypeBuilder<T> builder) where T : class, IUpdatable =>
        builder.IsUpdatable<T, User>();
}
