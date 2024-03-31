---
title: Solution structure
---

# Solution structure

## Folder structure

The solution has the following top-level folders:
- `assets` contains static assets, such as project logos
- `docs` contains the source for these docs
- `example` contains the source for the example project
- `src` contains the main package source code
- `test` contains the test projects

## MSBuild

The solution makes extensive use of MSBuild to standardise projects through build customisation.

### [eng/Build.props](gitfile://eng/Build.props)
The `Build.props` is imported early in the MSBuild sequence by [Directory.Build.props](gitfile://Directory.Build.props). It:
- Configures project info (e.g. product name, authors)
- Configures conventions (e.g. enabling nullable types)
- Enables .NET analysers
- Adds common third-party analysers
- Configures solution-wide warnings and errors
- Configures resources to be embedded in their respective assemblies

### [eng/Build.targets](gitfile://eng/Build.targets)
The `Build.props` is imported late in the MSBuild sequence by [Directory.Build.targets](gitfile://Directory.Build.targets). It:
- Contains common package versions
- Configures warnings and errors based on the project

### [eng/Packages.props](gitfile://eng/Packages.props)
- Marks projects to be published as packages
- Configures packaging properties
- This is imported at the end of the `.csproj`
- Sanity checks the projects

### [eng/Tests.props](gitfile://eng/Tests.props)
- Configures testing-based global usings
- Ignores some warnings not relevant to tests

### [eng/Version.props](gitfile://eng/Version.props)
- Configures the package version

## Docs
The docs app consits of 4 folders:
- `api` which DocFX spits Tyne's XML Documentation into
- `assets` for static assets, such as project logos
- `docs` where the bespoke documentation goes
- `template` where the alterations to the default template are added

### Table Of Contents
Each folder contains a Table Of Contents (`toc.yml`). This is what the sidebar uses to generate a naviation list for the site.

### Bespoke pages
The bespoke pages in Tyne's docs are broadly split into 3 categories:
- `docs/changes/*` are change notes for each minor version of Tyne. These contain a list of changes, a migration guide from the last minor version, and links to the git changes.
- `docs/dev/*` are docs (such as this one!) designed to aid people working on the Tyne solution.
- `docs/packages/*` are docs for individual packages.

## Example app
The example app is split across 4 projects:
- `Example.Data` hosts just the data model for the example project
- `Example.Client` contains the vast majority of the example project
- `Example.Host.Server` is solely responsible for hosting `Example.Client` in a Blazor Server app
    - Note that this is never deployed anywhere. Instead, it is useful when developing locally as the debugging experience is more seamless than when using Blazor WASM.
- `Example.Host.Wasm` is solely responsible for hosting `Example.Client` in a stand-alone Blazor WASM app
    - This is the version of the docs that is deployed. This is because stand-alone Blazor WASM apps can easily be deployed statically to GitHub pages.
