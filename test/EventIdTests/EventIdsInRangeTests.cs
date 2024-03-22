using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Tyne.Blazor;
using Tyne.EntityFramework;

namespace Tyne;

// Every Tyne package is included in here, regardless of whether it actually has an logger messages.
// This is to guard against adding logger messages, but forgetting to add them to this.
// For current ranges, see `/docs/dev/event-ids.md`.
// That doc should be updated if you make any changes to Event ID ranges.
public class EventIdsInRangeTests
{
    [Fact]
    public void Tyne_AspNetCore() =>
        AssertLoggerMessages.AreInRange(typeof(NotFoundEndpointExtensions).Assembly, 0..0);

    [Fact]
    public void Tyne_Blazor() =>
        AssertLoggerMessages.AreInRange(typeof(TyneKey).Assembly, 101_001_001..101_001_999);

    [Fact]
    public void Tyne_Core() =>
        AssertLoggerMessages.AreInRange(typeof(Result<>).Assembly, 0..0);

    [Fact]
    public void Tyne_EntityFramework() =>
        AssertLoggerMessages.AreInRange(typeof(BaseSearchHandler<,,>).Assembly, 0..0);

    [Fact]
    public void Tyne_EntityFramework_ChangeAuditing() =>
        AssertLoggerMessages.AreInRange(typeof(IDbContextChangeAuditor).Assembly, 0..0);

    [Fact]
    public void Tyne_EntityFramework_ModificationTracking() =>
        AssertLoggerMessages.AreInRange(typeof(IDbContextModificationTracker).Assembly, 0..0);

    [Fact]
    public void Tyne_EntityFramework_UserService_Core() =>
        AssertLoggerMessages.AreInRange(typeof(ServiceCollectionUserServiceExtensions).Assembly, 0..0);

    [Fact]
    public void Tyne_EntityFramework_UserService() =>
        AssertLoggerMessages.AreInRange(typeof(ITyneUserService).Assembly, 0..0);

    [Fact]
    public void Tyne_Testing() =>
        AssertLoggerMessages.AreInRange(typeof(TestingServiceCollectionModificationExtensions).Assembly, 0..0);
}
