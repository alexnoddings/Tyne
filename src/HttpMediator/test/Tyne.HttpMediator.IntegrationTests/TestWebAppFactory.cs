using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Tyne.HttpMediator.Client;

namespace Tyne.HttpMediator;

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
                    .WithMiddleware(static middleware =>
                        middleware
                        .UseExceptionHandler()
                        .UseFluentValidation()
                        .Send()
                    )
                );

            _ = services.AddScoped(_ => CreateClient());
        });
    }

    public IHttpMediator CreateHttpMediator() =>
        Services.GetRequiredService<IHttpMediator>();

    public ITestScope CreateTestScope() => new TestScope(Services.CreateScope());

    private sealed class TestScope : ITestScope
    {
        private readonly IServiceScope _serviceScope;

        public IHttpMediator HttpMediator { get; }
        public IServiceProvider Services => _serviceScope.ServiceProvider;

        public TestScope(IServiceScope serviceScope)
        {
            _serviceScope = serviceScope;
            HttpMediator = serviceScope.ServiceProvider.GetRequiredService<IHttpMediator>();
        }

        public void Deconstruct(out IHttpMediator httpMediator, out IServiceProvider services)
        {
            httpMediator = HttpMediator;
            services = Services;
        }

        public void Dispose() => _serviceScope.Dispose();
    }
}
