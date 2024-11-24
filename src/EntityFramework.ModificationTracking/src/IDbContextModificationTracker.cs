using Microsoft.EntityFrameworkCore;

namespace Tyne.EntityFramework;

public interface IDbContextModificationTracker
{
    public void TrackModifications(DbContext dbContext);
    public Task TrackModificationsAsync(DbContext dbContext, CancellationToken cancellationToken = default);
}
