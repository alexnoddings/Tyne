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
Tyne's common core types can be shortened using Tyne's [prelude system](../preludes.md):

```xml
<PropertyGroup>
    <!-- Enables every Tyne prelude -->
    <TynePrelude>enable</TynePrelude>
    <!------------  OR  ------------>
    <!-- Enables just Tyne's Core prelude -->
    <TyneCorePrelude>enable</TyneCorePrelude>
</PropertyGroup>
```

Once enabled:
- [`Result`](xref:Tyne.Result), [`Option`](xref:Tyne.Option), and [`Error`](xref:Tyne.Error) creation methods are imported statically:
    ```cs
    var okResult = Ok(42);
    var errorResult = Error<int>("No value");

    var some = Some(101);
    var none = None<int>();

    var error = Error("Nothing");
    ```
- [`Unit.Value`](xref:Tyne.Unit.Value) is imported statically as `unit`:
    ```cs
    // Without prelude
    return Unit.Value;
    // With prelude
    return unit;
    ```
