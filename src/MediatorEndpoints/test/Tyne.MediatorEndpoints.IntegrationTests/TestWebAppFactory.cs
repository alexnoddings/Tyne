using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace Tyne.MediatorEndpoints;

public class TestWebAppFactory : WebApplicationFactory<TestWebAppHost>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        _ = builder.ConfigureTestServices(services =>
        {
            _ = services
                .AddTyne()
                .AddClientHttpMediator(static builder =>
                    builder
                        .Configure(static options => options.ApiBase = TestWebAppHost.ApiBase)
                        .WithMiddleware(static middleware => middleware.Send())
                )
                .AddLegacyClientMediatorEndpointsCompatibility();

            _ = services.AddScoped(_ => CreateClient());
        });
    }

    public IMediatorProxy CreateMediatorProxy() =>
        Services.GetRequiredService<IMediatorProxy>();

    public ITestScope CreateTestScope() => new TestScope(Services.CreateScope());

    private sealed class TestScope : ITestScope
    {
        private readonly IServiceScope _serviceScope;

        public IMediatorProxy MediatorProxy { get; }
        public IServiceProvider Services => _serviceScope.ServiceProvider;

        public TestScope(IServiceScope serviceScope)
        {
            _serviceScope = serviceScope;
            MediatorProxy = serviceScope.ServiceProvider.GetRequiredService<IMediatorProxy>();
        }

        public void Deconstruct(out IMediatorProxy mediatorProxy, out IServiceProvider services)
        {
            mediatorProxy = MediatorProxy;
            services = Services;
        }

        public void Dispose() => _serviceScope.Dispose();
    }
}
