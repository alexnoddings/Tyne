---
title: Preludes
---

# Preludes
Multiple Tyne packages include a prelude. These are designed to make using Tyne more ergonomic.

## Enabling
Tyne preludes can be enabled by setting the `TynePrelude` property to `enable` in either your `project.csproj` or `Directory.Build.props`:

```xml
<PropertyGroup>
    <TynePrelude>enable</TynePrelude>
</PropertyGroup>
```

Alternatively, individual preludes can be enabled with their [respective properties](#packages-with-preludes).
Individual prelude properties take precidence over the general property. For example:

```xml
<PropertyGroup>
    <!-- Enables every Tyne prelude -->
    <TynePrelude>enable</TynePrelude>
    <!-- Disables the core prelude specifically -->
    <TyneCorePrelude>disable</TyneCorePrelude>
</PropertyGroup>
```

## Packages with preludes
- [Blazor](packages/Blazor.md#prelude)
- [Core](packages/Core.md#prelude)
