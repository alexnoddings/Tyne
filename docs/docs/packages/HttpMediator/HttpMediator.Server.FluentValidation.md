---
title: HttpMediator.Server.FluentValidation Package
---

# Tyne.HttpMediator.Server.FluentValidation

Fluent Validation middleware for Tyne's server HTTP Mediator.

## Installation

### Package

<div class="package-installation">

# [.NET CLI](#tab/dotnet-cli)
```shell
dotnet add package Tyne.MediatorEndpoints.Server.FluentValidation --version ${PACKAGE_VERSION}
```
# [PackageReference](#tab/package-reference)
```xml
<PackageReference Include="Tyne.MediatorEndpoints.Server.FluentValidation" Version="${PACKAGE_VERSION}" />
```
# [Package Manager](#tab/package-manager)
```powershell
Install-Package Tyne.MediatorEndpoints.Server.FluentValidation -Version ${PACKAGE_VERSION}
```
---

</div>

### Register services

Call [UseFluentValidation](xref:Microsoft.Extensions.DependencyInjection.HttpMediatorServerFluentValidationMiddlewareBuilderExtensions.UseFluentValidation*) as part of your server's HTTP mediator pipeline.

```cs
// Program.cs
builder.services
    .AddTyne()
    .AddServerHttpMediator(
        static builder => builder
            .UseExceptionHandler()
            // Adds the Fluent Validation middleware
            .UseFluentValidation()
            // other middleware
    );
```

See the [Fluent Validation docs](https://docs.fluentvalidation.net/en/latest/aspnet.html#getting-started) for registering your validators.

## Usage

The Fluent Validation middleware will execute registered `IValidator<TRequest>`s. If any fail, the pipeline will be short-circuited with a `BadRequest` result.
