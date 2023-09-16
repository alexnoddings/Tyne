---
title: Blazor Package
---

# Tyne.Blazor
Blazor component library.

## Installation

### Package

<div class="package-installation">

# [.NET CLI](#tab/dotnet-cli)
```shell
dotnet add package Tyne.Blazor --version ${PACKAGE_VERSION}
```
# [PackageReference](#tab/package-reference)
```xml
<PackageReference Include="Tyne.Blazor" Version="${PACKAGE_VERSION}" />
```
# [Package Manager](#tab/package-manager)
```powershell
Install-Package Tyne.Blazor -Version ${PACKAGE_VERSION}
```
---

</div>

### Script
Some of Tyne's Blazor components utilise JavaScript. To enable this, add the following script tag into your `index.html` or `_Layout.cshtml`:
```html
<script src="_content/Tyne.Blazor/Tyne.Blazor.js"></script>
```

### Prelude
Tyne's Blazor components are in separate namespaces.
These can be imported automatically using Tyne's [prelude system](../preludes.md):

```xml
<PropertyGroup>
    <!-- Enables every Tyne prelude -->
    <TynePrelude>enable</TynePrelude>
    <!------------  OR  ------------>
    <!-- Enables just Tyne's Blazor prelude -->
    <TyneBlazorPrelude>enable</TyneBlazorPrelude>
</PropertyGroup>
```

When enabled, Tyne's Blazor component namespaces will be added as [global usings](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/using-directive#global-modifier).
