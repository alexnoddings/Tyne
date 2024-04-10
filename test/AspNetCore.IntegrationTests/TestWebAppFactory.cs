using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace Tyne.AspNetCore;

public class TestWebAppFactory : WebApplicationFactory<TestWebAppHost>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            services.AddScoped(_ => CreateClient());
        });
    }
}
