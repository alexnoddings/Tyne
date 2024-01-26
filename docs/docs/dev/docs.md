---
title: Docs development
---

# Developing docs

## Getting started
Before getting started, execute `dotnet tool restore` in the solution root directory. This will restore the DocFX tools.

## Running

### Simple
The simple way to run the docs is executing `dotnet docfx ./docfx.json --serve` in the `docs` directory.
Note that the generated site is not dynamic - making any changes to the source will require stopping this command, and running it again.

### Advanced
The advanced way to run the docs is to use `dotnet watch`.

In the `docs` directory, execute `dotnet watch docfx .\docfx.json` in one terminal.
This will rebuild the DocFX project every time a change is detected to it's source files.

In a separate terminal in the `docs` directory, execute `dotnet docfx serve .\_site\` once the initial build above is complete.
This will serve the DocFX project on `http://localhost:8080`, and will stay available while the docs are rebuilding.

## Versioning
The publishing workflow is configured to populate build properties in the docs:
- The `"_appFooter": "Built locally"` config in `docfx.json` is replaced with build info
- The package version number will replace references to `${PACKAGE_VERSION }`, but without the space
    - The space character needs to be removed for the reference to be replaced, however doing so would cause the above text to also be replaced

## File referencing
The publishing workflow is configured to replace `gitfile` scheme references in the docs.
Any docs files which contain a link whose scheme is `gitfile` will have that link updated to point to the blob which the docs were build from.

For example, a markdown link to (without the space, otherwise it would be replaced here):
```md
[something](gitfile: //Directory.Build.props)
```

Will be replaced with:
```md
[something](gitfile://Directory.Build.props)
```

This custom scheme is relative to the root of the solution.
