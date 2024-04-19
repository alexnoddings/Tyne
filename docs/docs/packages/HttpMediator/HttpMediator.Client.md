---
title: HttpMediator.Client Package
---

# Tyne.HttpMediator.Client

Client implementation of Tyne's HTTP Mediator.

## Installation

<div class="package-installation">

# [.NET CLI](#tab/dotnet-cli)
```shell
dotnet add package Tyne.HttpMediator.Client --version ${PACKAGE_VERSION}
```
# [PackageReference](#tab/package-reference)
```xml
<PackageReference Include="Tyne.HttpMediator.Client" Version="${PACKAGE_VERSION}" />
```
# [Package Manager](#tab/package-manager)
```powershell
Install-Package Tyne.HttpMediator.Client -Version ${PACKAGE_VERSION}
```
---

</div>

### Register services

Call [AddClientHttpMediator](xref:Microsoft.Extensions.DependencyInjection.HttpMediatorClientTyneBuilderExtensions.AddClientHttpMediator*) after adding Tyne to register HTTP mediator services.

```cs
// Program.cs
builder.services
    .AddTyne()
    .AddClientHttpMediator(static builder =>
        builder
        // Optionally, configure the base URI (defaults to /api/)
        .Configure(static options => options.ApiBase = "...")
        .WithMiddleware(static middleware =>
            middleware
                // Catches any exceptions in the pipeline
                .UseExceptionHandler()
                // Add any other middleware here
                // ...
                // Sends the request to the server
                .Send()
        )
    );
```

## Usage

The [IHttpMediator](xref:Tyne.HttpMediator.Client.IHttpMediator) can be injected to [send requests to the server](xref:Tyne.HttpMediator.Client.IHttpMediator.SendAsync*).
