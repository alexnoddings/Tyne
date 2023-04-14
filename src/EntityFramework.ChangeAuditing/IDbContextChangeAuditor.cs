using Microsoft.EntityFrameworkCore;

namespace Tyne.EntityFramework;

public interface IDbContextChangeAuditor
{
    public void AuditChanges(DbContext dbContext);
    public Task AuditChangesAsync(DbContext dbContext, CancellationToken cancellationToken = default);
}
