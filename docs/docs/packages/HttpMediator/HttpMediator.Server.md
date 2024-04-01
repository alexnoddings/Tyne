---
title: HttpMediator.Server Package
---

# Tyne.HttpMediator.Server

Server implementation of Tyne's HTTP Mediator.

## Installation

<div class="package-installation">

# [.NET CLI](#tab/dotnet-cli)
```shell
dotnet add package Tyne.HttpMediator.Server --version ${PACKAGE_VERSION}
```
# [PackageReference](#tab/package-reference)
```xml
<PackageReference Include="Tyne.HttpMediator.Server" Version="${PACKAGE_VERSION}" />
```
# [Package Manager](#tab/package-manager)
```powershell
Install-Package Tyne.HttpMediator.Server -Version ${PACKAGE_VERSION}
```
---

</div>

### Register services

Call [AddServerHttpMediator](xref:Microsoft.Extensions.DependencyInjection.HttpMediatorServerTyneBuilderExtensions.AddServerHttpMediator*) after adding Tyne to register HTTP mediator services.

```cs
// Program.cs
builder.services
    .AddTyne()
    .AddServerHttpMediator(static builder =>
        builder
        // Optionally, configure the base URI (defaults to /api/)
        .Configure(static options => options.ApiBase = "...")
        .WithMiddleware(static middleware =>
            middleware
                // Catches any exceptions in the pipeline
                .UseExceptionHandler()
                // Add other middleware here
        )
    );
```

### Register endpoints
Call [MapHttpMediatorRequestsFromAssemblyContaining](xref:Microsoft.AspNetCore.Builder.WebApplicationEndpointMappingExtensions.MapHttpMediatorRequestsFromAssemblyContaining*) after adding Tyne to register HTTP mediator services.

```cs
// Program.cs
// Any requests in the assembly containing SomeRequest will be registered as endpoints
app.MapHttpMediatorRequestsFromAssemblyContaining<SomeRequest>();
```
