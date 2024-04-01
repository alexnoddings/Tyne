---
title: HttpMediator.Client.FluentValidation Package
---

# Tyne.HttpMediator.Client.FluentValidation

Fluent Validation middleware for Tyne's client HTTP Mediator.

## Installation

### Package

<div class="package-installation">

# [.NET CLI](#tab/dotnet-cli)
```shell
dotnet add package Tyne.MediatorEndpoints.Client.FluentValidation --version ${PACKAGE_VERSION}
```
# [PackageReference](#tab/package-reference)
```xml
<PackageReference Include="Tyne.MediatorEndpoints.Client.FluentValidation" Version="${PACKAGE_VERSION}" />
```
# [Package Manager](#tab/package-manager)
```powershell
Install-Package Tyne.MediatorEndpoints.Client.FluentValidation -Version ${PACKAGE_VERSION}
```
---

</div>

### Register services

Call [UseFluentValidation](xref:Microsoft.Extensions.DependencyInjection.HttpMediatorClientFluentValidationMiddlewareBuilderExtensions.UseFluentValidation*) as part of your client's HTTP mediator pipeline.

```cs
// Program.cs
builder.services
    .AddTyne()
    .AddClientHttpMediator(
        static builder => builder
            .UseExceptionHandler()
            // Adds the Fluent Validation middleware
            .UseFluentValidation()
            // any other middleware
            .Send()
    );
```

See the [Fluent Validation docs](https://docs.fluentvalidation.net/en/latest/aspnet.html#getting-started) for registering your validators.

## Usage

The Fluent Validation middleware will execute registered `IValidator<TRequest>`s before sending the request. If any fail, the pipeline will be short-circuited with a `BadRequest` result.
