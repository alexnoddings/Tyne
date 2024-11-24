using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Tyne.HttpMediator.Server;

public class WebApplicationEndpointMappingExtensionsTests
{
    private const string ApiBase = "/some-api/base/_path";

    [Fact]
    public void MapRequestsFromAssembly_MapsExampleRequestToExampleResponse()
    {
        // Arrange
        var webApplication = WebApplication.CreateBuilder().Build();

        // Act
        var endpointsBuilder = webApplication.MapHttpMediatorRequestsFromAssembly(typeof(SimpleRequest).Assembly);

        // Assert
        Assert_MapsExampleRequestToExampleResponse(endpointsBuilder);
    }

    [Fact]
    public void MapRequestsFromAssemblyContainingT_MapsExampleRequestToExampleResponse()
    {
        // Arrange
        var webApplication = WebApplication.CreateBuilder().Build();

        // Act
        var endpointsBuilder = webApplication.MapHttpMediatorRequestsFromAssemblyContaining<SimpleRequest>();

        // Assert
        Assert_MapsExampleRequestToExampleResponse(endpointsBuilder);
    }

    private static void Assert_MapsExampleRequestToExampleResponse(IEndpointRouteBuilder endpointsBuilder)
    {
        var routeEndpoints =
            endpointsBuilder.DataSources
            .SelectMany(dataSource => dataSource.Endpoints)
            .OfType<RouteEndpoint>();

        Assert.Contains(routeEndpoints, endpoint => endpoint.RoutePattern.RawText?.Contains(SimpleRequest.Uri, StringComparison.Ordinal) == true);
    }

    [Fact]
    public void MapRequestsFromAssembly_HasCorrectApiBase()
    {
        // Arrange
        var webApplicationBuilder = WebApplication.CreateBuilder();
        _ = webApplicationBuilder.Services
            .AddTyne()
            .AddServerHttpMediator(static builder =>
                builder.Configure(static options => options.ApiBase = ApiBase)
            );

        var webApplication = webApplicationBuilder.Build();

        // Act
        var endpointsBuilder = webApplication.MapHttpMediatorRequestsFromAssembly(typeof(SimpleRequest).Assembly);

        // Assert
        var expectedRoutePattern = $"{ApiBase}/{SimpleRequest.Uri}";

        var routeEndpoints = endpointsBuilder.DataSources
            .SelectMany(dataSource => dataSource.Endpoints)
            .OfType<RouteEndpoint>();

        Assert.Contains(routeEndpoints, endpoint => endpoint.RoutePattern.RawText == expectedRoutePattern);
    }

    public static IEnumerable<object?[]> MapRequestFromAssembly_MapsHttpVerbsCorrectly_Data()
    {
        yield return new object?[] { HttpDeleteRequest.Uri, HttpMethod.Delete };
        yield return new object?[] { HttpGetRequest.Uri, HttpMethod.Get };
        yield return new object?[] { HttpPatchRequest.Uri, HttpMethod.Patch };
        yield return new object?[] { HttpPostRequest.Uri, HttpMethod.Post };
        yield return new object?[] { HttpPutRequest.Uri, HttpMethod.Put };
    }

    [Theory]
    [MemberData(nameof(MapRequestFromAssembly_MapsHttpVerbsCorrectly_Data))]
    [SuppressMessage("Design", "CA1054: URI-like parameters should not be strings.", Justification = "The tested code uses strings.")]
    public void MapRequestFromAssembly_MapsHttpVerbsCorrectly(string uri, HttpMethod httpMethod)
    {
        // Arrange
        var webApplication = WebApplication.CreateBuilder().Build();

        // Act
        var endpointsBuilder = webApplication.MapHttpMediatorRequestsFromAssemblyContaining<SimpleRequest>();

        // Assert
        var routeEndpoints =
            endpointsBuilder.DataSources
            .SelectMany(dataSource => dataSource.Endpoints)
            .OfType<RouteEndpoint>();

        Assert.Contains(routeEndpoints, endpoint =>
            endpoint.RoutePattern.RawText?.Contains(uri, StringComparison.Ordinal) == true
            && endpoint.Metadata.OfType<HttpMethodMetadata>().Any(metadata => metadata.HttpMethods.Contains(httpMethod.Method))
        );
    }
}
