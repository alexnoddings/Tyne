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
The publishing workflow is configured to populate build properties in the docs. These are:
- The `"_appFooter": "Built locally"` config in `docfx.json` is replaced with build info
- Any reference to `${PACKAGE_VERSION}` is replaced with the version number
