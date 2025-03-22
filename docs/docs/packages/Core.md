---
title: Core Package
---

# Tyne.Core
Core functionality for other Tyne packages.

Common types:
- [`Result<T>`](xref:Tyne.Result`1) (and [extensions](xref:Tyne.ResultExtensions))
- [`Option<T>`](xref:Tyne.Option`1) (and [extensions](xref:Tyne.OptionExtensions))
- [`Error`](xref:Tyne.Error) (and [extensions](xref:Tyne.ErrorExtensions))

## Installation

<div class="package-installation">

# [.NET CLI](#tab/dotnet-cli)
```shell
dotnet add package Tyne.Core --version ${PACKAGE_VERSION}
```
# [PackageReference](#tab/package-reference)
```xml
<PackageReference Include="Tyne.Core" Version="${PACKAGE_VERSION}" />
```
# [Package Manager](#tab/package-manager)
```powershell
Install-Package Tyne.Core -Version ${PACKAGE_VERSION}
```
---

</div>

### Prelude
Tyne's common core types can be shortened using Tyne's prelude system, which is designed to make using common types more ergonomic.

Tyne's core prelude is enabled by default, but can be controlled with the `TynePrelude` property:

```xml
<PropertyGroup>
    <!-- Disables the Tyne prelude -->
    <TynePrelude>disable</TynePrelude>
    <!-- Enables the Tyne prelude (default setting) -->
    <TynePrelude>enable</TynePrelude>
</PropertyGroup>
```

Preludes come enabled by default. When enabled:
- [`Unit.Value`](xref:Tyne.Unit.Value) is imported statically as `unit`:
    ```cs
    // Without prelude
    return Unit.Value;

    // With prelude
    return unit;
    ```

- [`Result`](xref:Tyne.Result`2) creation methods are imported statically:
    ```cs
    // Without prelude
    var okResult = Result.Ok<int, string>(42);
    var errorResult = Result.Error<int, string>("some error");

    // With prelude
    var okResult = Ok<int, string>(42);
    var errorResult = Error<int, string>("No value");
    ```

- [`Option`](xref:Tyne.Option`1) creation methods are imported statically:
    ```cs
    // Without prelude
    var some = Option.Some(101);
    var none = Option.None<int>();

    // With prelude
    var some = Some(101);
    var none = None<int>();
    ```
