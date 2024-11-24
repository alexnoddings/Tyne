using Microsoft.Extensions.DependencyInjection;
using Tyne.EntityFramework;

namespace Tyne;

public interface IDbContextChangeAuditingBuilder<TEvent, TProperty, TRelation>
    where TEvent : DbContextChangeEvent<TEvent, TProperty, TRelation>, new()
    where TProperty : DbContextChangeEventProperty<TEvent, TProperty, TRelation>, new()
    where TRelation : DbContextChangeEventRelation<TEvent, TProperty, TRelation>, new()
{
    /// <summary>
    ///     Configures change auditing to use the <typeparamref name="TAuditor"/> auditor as a <see cref="ServiceLifetime.Scoped"/> service.
    /// </summary>
    /// <typeparam name="TAuditor">
    ///     The type of auditor.
    /// </typeparam>
    /// <returns>
    ///     This builder for chaining.
    /// </returns>
    public IDbContextChangeAuditingBuilder<TEvent, TProperty, TRelation> UseAuditor<TAuditor>()
        where TAuditor : DbContextChangeAuditor<TEvent, TProperty, TRelation>;

    /// <summary>
    ///     Configures change auditing to use the <typeparamref name="TAuditor"/> auditor as a <paramref name="serviceLifetime"/> service.
    /// </summary>
    /// <typeparam name="TAuditor">
    ///     The type of auditor.
    /// </typeparam>
    /// <param name="serviceLifetime">
    ///     The <see cref="ServiceLifetime"/> to use for <typeparamref name="TAuditor"/>.
    /// </param>
    /// <returns>
    ///     This builder for chaining.
    /// </returns>
    public IDbContextChangeAuditingBuilder<TEvent, TProperty, TRelation> UseAuditor<TAuditor>(ServiceLifetime serviceLifetime)
        where TAuditor : DbContextChangeAuditor<TEvent, TProperty, TRelation>;

    /// <summary>
    ///     Configures change auditing to use <paramref name="instance"/> as the auditor.
    /// </summary>
    /// <typeparam name="TAuditor">
    ///     The type of auditor.
    /// </typeparam>
    /// <param name="instance">
    ///     An instance of <typeparamref name="TAuditor"/>.
    /// </param>
    /// <returns>
    ///     This builder for chaining.
    /// </returns>
    public IDbContextChangeAuditingBuilder<TEvent, TProperty, TRelation> UseAuditor<TAuditor>(TAuditor instance)
        where TAuditor : DbContextChangeAuditor<TEvent, TProperty, TRelation>;

    /// <summary>
    ///     Configures change auditing to use the <typeparamref name="TAuditor"/> auditor as a <see cref="ServiceLifetime.Scoped"/> service, created using <paramref name="factory"/>.
    /// </summary>
    /// <typeparam name="TAuditor">
    ///     The type of auditor.
    /// </typeparam>
    /// <param name="factory">
    ///     A function to resolve a <typeparamref name="TAuditor"/> from a <see cref="IServiceProvider"/>.
    /// </param>
    /// <returns>
    ///     This builder for chaining.
    /// </returns>
    public IDbContextChangeAuditingBuilder<TEvent, TProperty, TRelation> UseAuditor<TAuditor>(Func<IServiceProvider, TAuditor> factory)
        where TAuditor : DbContextChangeAuditor<TEvent, TProperty, TRelation>;

    /// <summary>
    ///     Configures change auditing to use the <typeparamref name="TAuditor"/> auditor as a <paramref name="serviceLifetime"/> service, created using <paramref name="factory"/>.
    /// </summary>
    /// <typeparam name="TAuditor">
    ///     The type of auditor.
    /// </typeparam>
    /// <param name="factory">
    ///     A function to resolve a <typeparamref name="TAuditor"/> from a <see cref="IServiceProvider"/>.
    /// </param>
    /// <param name="serviceLifetime">
    ///     The <see cref="ServiceLifetime"/> to use for <typeparamref name="TAuditor"/>.
    /// </param>
    /// <returns>
    ///     This builder for chaining.
    /// </returns>
    public IDbContextChangeAuditingBuilder<TEvent, TProperty, TRelation> UseAuditor<TAuditor>(Func<IServiceProvider, TAuditor> factory, ServiceLifetime serviceLifetime)
        where TAuditor : DbContextChangeAuditor<TEvent, TProperty, TRelation>;
}
