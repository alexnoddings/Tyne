using Tyne.Aerospace.Data;
using Tyne.EntityFramework;

namespace Tyne.Aerospace.Client.Infrastructure.Data;

public sealed class SqliteInMemoryDbContextFactory : IAppDbContextFactory
{
    private readonly SqliteInMemoryConnectionFactory _connectionFactory;
    private readonly IDbContextModificationTracker _modificationTracker;

    public SqliteInMemoryDbContextFactory(SqliteInMemoryConnectionFactory connectionFactory, IDbContextModificationTracker modificationTracker)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        _modificationTracker = modificationTracker ?? throw new ArgumentNullException(nameof(modificationTracker));
    }

    public AppDbContext CreateDbContext()
    {
        var options = _connectionFactory.CreateOptions();
        var context = new AppDbContext(options, _modificationTracker);
        DataSeeder.EnsureSeeded(context);

        return context;
    }
}
