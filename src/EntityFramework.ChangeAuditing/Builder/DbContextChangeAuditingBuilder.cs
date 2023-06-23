using Microsoft.Extensions.DependencyInjection;

namespace Tyne.EntityFramework.Builder;

internal sealed class DbContextChangeAuditingBuilder<TEvent, TProperty, TRelation>
    : IDbContextChangeAuditingBuilder<TEvent, TProperty, TRelation>
    where TEvent : DbContextChangeEvent<TEvent, TProperty, TRelation>, new()
    where TProperty : DbContextChangeEventProperty<TEvent, TProperty, TRelation>, new()
    where TRelation : DbContextChangeEventRelation<TEvent, TProperty, TRelation>, new()
{
    internal ServiceDescriptor? AuditorServiceDescriptor { get; set; }

    public IDbContextChangeAuditingBuilder<TEvent, TProperty, TRelation> UseAuditor<TAuditor>()
        where TAuditor : DbContextChangeAuditor<TEvent, TProperty, TRelation> =>
        UseAuditor<TAuditor>(ServiceLifetime.Scoped);

    public IDbContextChangeAuditingBuilder<TEvent, TProperty, TRelation> UseAuditor<TAuditor>(ServiceLifetime serviceLifetime)
        where TAuditor : DbContextChangeAuditor<TEvent, TProperty, TRelation>
    {
        EnsureNotConfigured();

        AuditorServiceDescriptor = new(typeof(IDbContextChangeAuditor), typeof(TAuditor), serviceLifetime);
        return this;
    }

    public IDbContextChangeAuditingBuilder<TEvent, TProperty, TRelation> UseAuditor<TAuditor>(TAuditor instance)
        where TAuditor : DbContextChangeAuditor<TEvent, TProperty, TRelation>
    {
        EnsureNotConfigured();

        AuditorServiceDescriptor = new(typeof(IDbContextChangeAuditor), instance);
        return this;
    }

    public IDbContextChangeAuditingBuilder<TEvent, TProperty, TRelation> UseAuditor<TAuditor>(Func<IServiceProvider, TAuditor> factory)
        where TAuditor : DbContextChangeAuditor<TEvent, TProperty, TRelation> =>
        UseAuditor(factory, ServiceLifetime.Scoped);

    public IDbContextChangeAuditingBuilder<TEvent, TProperty, TRelation> UseAuditor<TAuditor>(Func<IServiceProvider, TAuditor> factory, ServiceLifetime serviceLifetime)
        where TAuditor : DbContextChangeAuditor<TEvent, TProperty, TRelation>
    {
        EnsureNotConfigured();

        AuditorServiceDescriptor = new(typeof(IDbContextChangeAuditor), factory, serviceLifetime);
        return this;
    }

    private void EnsureNotConfigured()
    {
        if (AuditorServiceDescriptor is not null)
            throw new InvalidOperationException("Auditor already configured.");
    }
}
