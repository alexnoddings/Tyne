using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace Tyne.SourceRef.SourceGenerators.Components;

internal static class ComponentSourceInfoCollector
{
    /// <summary>
    ///     Collects <see cref="SourceInfo"/>s about components from <paramref name="files"/>.
    /// </summary>
    public static IEnumerable<SourceInfo> Collect(IEnumerable<AdditionalText> files, string rootNamespace, string projectPath)
    {
        foreach (var file in files)
        {
            var fileName = Path.GetFileName(file.Path);

            // This collector only cares about component source code
            if (!fileName.EndsWith(".razor", StringComparison.OrdinalIgnoreCase))
                yield break;

            var componentName = GetComponentName(file);
            var componentNamespace =
                TryGetNamespaceFromDirective(file)
                ?? GetNamespaceFromConvention(file, rootNamespace, projectPath);

            if (componentNamespace is null)
                yield break;

            // Build the type's full identifier
            var identifier = componentNamespace + "." + componentName;

            // Get the absolute path to the type's definition
            var absolutePath = file.Path;

            // Get the type's source - there's nothing fancy involved here, just reading the file's contents
            var componentSource = file.GetText()?.ToString() ?? string.Empty;

            // Format the source
            var componentSourceFormatted = ComponentSourceFormatter.Format(componentSource);

            yield return new SourceInfo(identifier, absolutePath, componentSourceFormatted);
        }
    }

    /// <summary>
    ///     Gets the name of a component based on the name of the file.
    /// </summary>
    /// <remarks>
    ///     <code>
    ///     // Has absolute path "D:/Tyne/example/Some/Component.razor"
    ///     var file = ...
    ///     // Returns "Component"
    ///     GetComponentName(file);
    ///     </code>
    /// </remarks>
    private static string GetComponentName(AdditionalText file)
    {
        // Get the component's full file name
        var componentFullFileName = Path.GetFileName(file.Path);
        // The component's name is everything before the first extension
        var componentNameEnd = componentFullFileName.IndexOf('.');
        return componentFullFileName.Substring(0, componentNameEnd);
    }

    // Searches for:
    // - line start
    // - "@namespace"
    // - at least one whitespace
    // - an identifier comprised of (a-z, 0-9, ., or _)
    // - any amount of whitespace
    // - line end
    private static readonly Regex _razorNamespaceDeclarationRegex =
        new(pattern: "^@namespace\\s+(?<NamespaceIdentifier>[a-z0-9._]+)\\s*$",
            options: RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

    /// <summary>
    ///     Tries to get a component's namespace from a "@namespace X.Y.Z" directive.
    ///     Will return <see langword="null"/> if no directive is found.
    /// </summary>
    private static string? TryGetNamespaceFromDirective(AdditionalText file)
    {
        var fileLines = file.GetText()?.Lines;
        if (fileLines is null)
            return null;

        var namespaceDeclarationRegex = _razorNamespaceDeclarationRegex;

        foreach (var line in fileLines)
        {
            var match = namespaceDeclarationRegex.Match(line.Text?.ToString());
            if (!match.Success)
                continue;

            return match.Groups["NamespaceIdentifier"].Value;
        }

        return null;
    }

    /// <summary>
    ///     Gets a component's namespace from it's file structure convention.
    /// </summary>
    /// <remarks>
    ///     The namespace is built by appending the project's root namespace
    ///     with the relative path to the file. E.g.:
    ///     <code>
    ///     // Has absolute path "D:/Tyne/example/Some/File.razor"
    ///     var file = ...
    ///     // Returns "Tyne.Example.Some"
    ///     GetNamespaceFromConvention(file, "Tyne.Example", "D:/Tyne/example");
    ///     </code>
    /// </remarks>
    private static string? GetNamespaceFromConvention(AdditionalText file, string? rootNamespace, string projectPath)
    {
        // Get the path relative to the root of the project
        var componentRelativePath = GetPathRelativeToProject(file.Path, projectPath);

        // Gets the length of the path up to the file name
        var lastPathSeparator = componentRelativePath.LastIndexOf(Path.DirectorySeparatorChar);

        // Gets the relative directory to this component from the root of the project
        var componentRelativeDirectory =
            lastPathSeparator > 0
            ? componentRelativePath.Substring(0, lastPathSeparator)
            : string.Empty;

        // Replace directory chars with .s (Some/Example/Path -> Some.Example.Path)
        var componentRelativeNamespace = componentRelativeDirectory.Replace(Path.DirectorySeparatorChar, '.');

        // Combine the project's root namespace with our relative namespace (and trim in case the root or relative namespaces are empty)
        return (rootNamespace + "." + componentRelativeNamespace).Trim('.');
    }

    /// <summary>
    ///     Makes <paramref name="path"/> relative to <paramref name="projectPath"/>.
    /// </summary>
    /// <remarks>
    ///     <code>
    ///     // Returns "/Some/File.cs"
    ///     GetPathRelativeToProject("D:/Tyne/example/Some/File.cs", "D:/Tyne/example");
    ///     </code>
    /// </remarks>
    private static string GetPathRelativeToProject(string path, string projectPath)
    {
        if (!path.StartsWith(projectPath, StringComparison.Ordinal))
            throw new ArgumentException($"Path \"{path}\" is not in the project.", nameof(path));

        return path.Substring(projectPath.Length);
    }
}
