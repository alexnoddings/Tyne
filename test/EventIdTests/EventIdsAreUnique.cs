using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Tyne.Blazor;
using Tyne.EntityFramework;

namespace Tyne;

// Every Tyne package is included in here, regardless of whether it actually has an logger messages.
// This is to guard against adding logger messages, but forgetting to add them to this.
public class EventIdsAreUnique
{
    [Fact]
    public void Tyne_AspNetCore() =>
        AssertLoggerMessages.AreUnique(typeof(NotFoundEndpointExtensions).Assembly);

    [Fact]
    public void Tyne_Blazor() =>
        AssertLoggerMessages.AreUnique(typeof(TyneKey).Assembly);

    [Fact]
    public void Tyne_Core() =>
        AssertLoggerMessages.AreUnique(typeof(Result<>).Assembly);

    [Fact]
    public void Tyne_EntityFramework() =>
        AssertLoggerMessages.AreUnique(typeof(BaseSearchHandler<,,>).Assembly);

    [Fact]
    public void Tyne_EntityFramework_ChangeAuditing() =>
        AssertLoggerMessages.AreUnique(typeof(IDbContextChangeAuditor).Assembly);

    [Fact]
    public void Tyne_EntityFramework_ModificationTracking() =>
        AssertLoggerMessages.AreUnique(typeof(IDbContextModificationTracker).Assembly);

    [Fact]
    public void Tyne_EntityFramework_UserService_Core() =>
        AssertLoggerMessages.AreUnique(typeof(ServiceCollectionUserServiceExtensions).Assembly);

    [Fact]
    public void Tyne_EntityFramework_UserService() =>
        AssertLoggerMessages.AreUnique(typeof(ITyneUserService).Assembly);

    [Fact]
    public void Tyne_Testing() =>
        AssertLoggerMessages.AreUnique(typeof(TestingServiceCollectionModificationExtensions).Assembly);
}
