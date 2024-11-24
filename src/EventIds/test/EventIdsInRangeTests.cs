using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Tyne.Blazor;
using Tyne.EntityFramework;
using Tyne.HttpMediator;
using Tyne.HttpMediator.Client;
using Tyne.HttpMediator.Server;
using ClientFluentValidationMiddleware = Tyne.HttpMediator.Client.FluentValidationMiddleware;
using ServerFluentValidationMiddleware = Tyne.HttpMediator.Server.FluentValidationMiddleware;

namespace Tyne;

// Every Tyne package is included in here, regardless of whether it actually has an logger messages.
// This is to guard against adding logger messages, but forgetting to add them to this.
// For current ranges, see `/docs/dev/event-ids.md`.
// That doc should be updated if you make any changes to Event ID ranges.
public class EventIdsInRangeTests
{
    [Fact]
    public void Tyne_AspNetCore() =>
        AssertLoggerMessages.AreInRange(typeof(NotFoundEndpointExtensions).Assembly, ..0);

    [Fact]
    public void Tyne_Blazor() =>
        AssertLoggerMessages.AreInRange(typeof(TyneKey).Assembly, 101_001_000..101_001_999);

    [Fact]
    public void Tyne_Core() =>
        AssertLoggerMessages.AreInRange(typeof(Result<>).Assembly, ..0);

    [Fact]
    public void Tyne_EntityFramework() =>
        AssertLoggerMessages.AreInRange(typeof(BaseSearchHandler<,,>).Assembly, ..0);

    [Fact]
    public void Tyne_EntityFramework_ChangeAuditing() =>
        AssertLoggerMessages.AreInRange(typeof(IDbContextChangeAuditor).Assembly, ..0);

    [Fact]
    public void Tyne_EntityFramework_ModificationTracking() =>
        AssertLoggerMessages.AreInRange(typeof(IDbContextModificationTracker).Assembly, ..0);

    [Fact]
    public void Tyne_EntityFramework_UserService_Core() =>
        AssertLoggerMessages.AreInRange(typeof(ServiceCollectionUserServiceExtensions).Assembly, ..0);

    [Fact]
    public void Tyne_EntityFramework_UserService() =>
        AssertLoggerMessages.AreInRange(typeof(ITyneUserService).Assembly, ..0);

    [Fact]
    public void Tyne_HttpMediator_Client() =>
        AssertLoggerMessages.AreInRange(typeof(IHttpMediator).Assembly, 101_002_000..101_002_999);

    [Fact]
    public void Tyne_HttpMediator_Client_FluentValidation() =>
        AssertLoggerMessages.AreInRange(typeof(ClientFluentValidationMiddleware).Assembly, 101_004_000..101_004_999);

    [Fact]
    public void Tyne_HttpMediator_Core() =>
        AssertLoggerMessages.AreInRange(typeof(HttpResult<>).Assembly, ..0);

    [Fact]
    public void Tyne_HttpMediator_Server() =>
        AssertLoggerMessages.AreInRange(typeof(HttpMediator.Server.HttpMediatorDelegate<,>).Assembly, 101_003_000..101_003_999);

    [Fact]
    public void Tyne_HttpMediator_Server_FluentValidation() =>
        AssertLoggerMessages.AreInRange(typeof(ServerFluentValidationMiddleware).Assembly, ..0);

    [Fact]
    public void Tyne_HttpMediator_Server_MediatR() =>
        AssertLoggerMessages.AreInRange(typeof(MediatRSenderMiddleware).Assembly, ..0);

    [Fact]
    public void Tyne_Testing() =>
        AssertLoggerMessages.AreInRange(typeof(TestingServiceCollectionModificationExtensions).Assembly, ..0);
}
