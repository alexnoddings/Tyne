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

### Style sheet
Some of Tyne's Blazor components contain custom styling. These needs the following reference adding to your `index.html` or `_Layout.cshtml`:
```html
<link rel="stylesheet" href="_content/Tyne.Blazor/Tyne.Blazor.css" />
```

### Script
Some of Tyne's Blazor components utilise JavaScript. To enable this, add the following script tag into your `index.html` or `_Layout.cshtml`:
```html
<script src="_content/Tyne.Blazor/Tyne.Blazor.js"></script>
```

## Components
See [the components](./Blazor/components.md) provided by this package.
