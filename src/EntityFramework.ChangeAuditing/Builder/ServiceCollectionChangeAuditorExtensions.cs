using Microsoft.Extensions.DependencyInjection;
using Tyne.EntityFramework;
using Tyne.EntityFramework.Builder;

namespace Tyne;

public static class ServiceCollectionChangeAuditorExtensions
{
    /// <summary>
    ///     Adds DbContext change auditing.
    /// </summary>
    /// <typeparam name="TEvent">The type of <see cref="DbContextChangeEvent{TEvent, TProperty, TRelation}"/>.</typeparam>
    /// <typeparam name="TProperty">The type of <see cref="DbContextChangeEventProperty{TEvent, TProperty, TRelation}"/>.</typeparam>
    /// <typeparam name="TRelation">The type of <see cref="DbContextChangeEventRelation{TEvent, TProperty, TRelation}"/>.</typeparam>
    /// <param name="builder">The <see cref="TyneBuilder"/>.</param>
    /// <returns>The <paramref name="builder"/> for chaining.</returns>
    /// <remarks>
    ///     <para>
    ///         While usable, it is usually more convenient to inherit from the entities in your own code. This makes modifying and referencing them much simpler.
    ///     </para>
    ///     <para>
    ///         This overload does not configure DbContext change auditing. Call <see cref="AddDbContextChangeAuditor{TEvent, TProperty, TRelation}(TyneBuilder, Action{IDbContextChangeAuditingBuilder{TEvent, TProperty, TRelation}})"/> to configure the auditor type.
    ///     </para>
    /// </remarks>
    public static TyneBuilder AddDbContextChangeAuditor<TEvent, TProperty, TRelation>(this TyneBuilder builder)
        where TEvent : DbContextChangeEvent<TEvent, TProperty, TRelation>, new()
        where TProperty : DbContextChangeEventProperty<TEvent, TProperty, TRelation>, new()
        where TRelation : DbContextChangeEventRelation<TEvent, TProperty, TRelation>, new() =>
        AddDbContextChangeAuditor<TEvent, TProperty, TRelation>(builder, _ => { });

    /// <summary>
    ///     Adds DbContext change auditing.
    /// </summary>
    /// <typeparam name="TEvent">The type of <see cref="DbContextChangeEvent{TEvent, TProperty, TRelation}"/>.</typeparam>
    /// <typeparam name="TProperty">The type of <see cref="DbContextChangeEventProperty{TEvent, TProperty, TRelation}"/>.</typeparam>
    /// <typeparam name="TRelation">The type of <see cref="DbContextChangeEventRelation{TEvent, TProperty, TRelation}"/>.</typeparam>
    /// <param name="builder">The <see cref="TyneBuilder"/>.</param>
    /// <param name="configure">Configures change auditing.</param>
    /// <returns>The <paramref name="builder"/> for chaining.</returns>
    /// <remarks>
    ///     While usable, it is usually more convenient to inherit from the entities in your own code. This makes modifying and referencing them much simpler.
    /// </remarks>
    public static TyneBuilder AddDbContextChangeAuditor<TEvent, TProperty, TRelation>(this TyneBuilder builder, Action<IDbContextChangeAuditingBuilder<TEvent, TProperty, TRelation>> configure)
        where TEvent : DbContextChangeEvent<TEvent, TProperty, TRelation>, new()
        where TProperty : DbContextChangeEventProperty<TEvent, TProperty, TRelation>, new()
        where TRelation : DbContextChangeEventRelation<TEvent, TProperty, TRelation>, new()
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configure);

        if (!builder.Services.Any(serviceDescriptor => serviceDescriptor.ServiceType == typeof(ITyneUserService)))
            throw new InvalidOperationException($"Please ensure an {nameof(ITyneUserService)} is registered before calling {nameof(AddDbContextChangeAuditor)}.");

        var auditingBuilder = new DbContextChangeAuditingBuilder<TEvent, TProperty, TRelation>();
        configure(auditingBuilder);

        var auditorServiceDescriptor =
            auditingBuilder.AuditorServiceDescriptor
            // If no auditor is configured, fall back to the default implementation
            ?? new(typeof(IDbContextChangeAuditor), typeof(DbContextChangeAuditor<TEvent, TProperty, TRelation>), ServiceLifetime.Scoped);

        builder.Services.Add(auditorServiceDescriptor);

        return builder;
    }
}
