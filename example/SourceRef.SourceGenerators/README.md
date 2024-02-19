# SourceRef source generators
SourceRef is used by the `Example.Client` project to display the example component/type source code in-line with the example.
It source generates classes in the target assembly which have a field for each type defined in the assembly.
The content of these fields is either a path to where the type is defined, or the source of the type's definition.

Components and types are handled separately to support components with code-behind files.

If modifying the source generators, keep in mind that MSBuild caches generators, meaning changes made won't be accurately reflected.
You need to force the MSBuild service to shutdown (clearing it's cache) before building like so:
```bash
dotnet build-server shutdown && dotnet clean && dotnet build
```

You can also debug source generator outputs by adding the `EmitCompilerGeneratedFiles` property to a project file:
```xml
<PropertyGroup>
    <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
</PropertyGroup>
```
