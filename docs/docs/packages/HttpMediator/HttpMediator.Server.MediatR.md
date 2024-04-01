---
title: HttpMediator.Server.MediatR Package
---

# Tyne.HttpMediator.Server.MediatR

MediatR middleware for Tyne's server HTTP Mediator.

## Installation

### Package

<div class="package-installation">

# [.NET CLI](#tab/dotnet-cli)
```shell
dotnet add package Tyne.MediatorEndpoints.Server.MediatR --version ${PACKAGE_VERSION}
```
# [PackageReference](#tab/package-reference)
```xml
<PackageReference Include="Tyne.MediatorEndpoints.Server.MediatR" Version="${PACKAGE_VERSION}" />
```
# [Package Manager](#tab/package-manager)
```powershell
Install-Package Tyne.MediatorEndpoints.Server.MediatR -Version ${PACKAGE_VERSION}
```
---

</div>

### Register services

Call [UseMediatR](xref:Microsoft.Extensions.DependencyInjection.HttpMediatorServerMediatRMiddlewareBuilderExtensions.UseMediatR*) as part of your server's HTTP mediator pipeline.

```cs
// Program.cs
builder.services
    .AddTyne()
    .AddServerHttpMediator(
        static builder => builder
            .UseExceptionHandler()
            // Adds the MediatR middleware
            .UseMediatR()
    );
```

See the [MediatR docs](https://github.com/jbogard/MediatR/wiki#setup) for registering your handlers.

## Usage

The MediatR middleware will act as a terminal middleware for any `TRequest`s which implement `IRequest<HttpResult<TResponse>>`. Other requests will be passed to the next middleware.

Handlers may choose to use [IHttpRequestHandler](xref:Tyne.HttpMediator.Server.IHttpRequestHandler`2) to simplify interface definitions.
