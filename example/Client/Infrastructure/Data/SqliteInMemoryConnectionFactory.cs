using System.Diagnostics.CodeAnalysis;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Tyne.Aerospace.Data;

namespace Tyne.Aerospace.Client.Infrastructure.Data;

public sealed class SqliteInMemoryConnectionFactory : IDisposable
{
    private readonly object _initialisationLock = new();
    private SqliteConnection? _connection;

    [MemberNotNull(nameof(_connection))]
    private void EnsureConnectionInitialised()
    {
        if (_connection is null)
        {
            // We can't use semaphores here as this is executed in a synchronous context,
            // and SemaphoreSlim.Wait will deadlock browsers
            lock (_initialisationLock)
            {
                if (_connection is null)
                {
                    // If not set as `:memory:?cache=shared`, the db will be scoped to this instance/connection
                    _connection = new SqliteConnection("Filename=:memory:");
                    _connection.Open();
                }
            }
        }
    }

    public DbContextOptions<AppDbContext> CreateOptions()
    {
        EnsureConnectionInitialised();
        return new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}
