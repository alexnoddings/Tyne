namespace Tyne.SourceRef.SourceGenerators.Components;

internal static class ComponentSourceFormatter
{
    private static readonly string[] _fileContentsSplit = ["\n"];

    /// <summary>
    ///     Formats the source code of a Razor component.
    /// </summary>
    public static string Format(string contents)
    {
        // First split the source by newline, keep the empty entries
        var split = contents.Replace("\r\n", "\n").Split(_fileContentsSplit, StringSplitOptions.None);

        // Strip component directives
        var stripped = StripComponentDirectives(split);

        // Escape the content to make it safe to put in a string
        var escaped =
            stripped
            // Escape '\'s
            .Select(line => line.Replace("\\", "\\\\"))
            // Escape '"'s
            .Select(line => line.Replace("\"", "\\\""))
            .ToList();

        // Joins by escaped newlines, and trim any excess whitespace from the end
        return string.Join("\\n", escaped).TrimEnd();
    }

    // Strips directives from the top of components, these aren't useful in example source
    // - Removes "@page ..."
    // - Removes "@using A.B"
    // - Removes "@namespace X.Y"
    // - Takes any whitespace off of the top of the file
    private static IEnumerable<string> StripComponentDirectives(IEnumerable<string> lines)
    {
        var hasYieldedLine = false;

        foreach (var line in lines)
        {
            // These directives just clutter up examples
            var isKnownDirective =
                line.StartsWith("@page", StringComparison.OrdinalIgnoreCase)
                || line.StartsWith("@using", StringComparison.OrdinalIgnoreCase)
                || line.StartsWith("@layout", StringComparison.OrdinalIgnoreCase)
                || line.StartsWith("@namespace", StringComparison.OrdinalIgnoreCase);

            if (isKnownDirective)
                continue;

            // We purposefully don't skip the inject, attribute, or implements
            // directives as these could all be relevant to an example

            // We want to preserve empty lines in the body of the file,
            // but not before the file has began (i.e. between directives and the actual source code)
            if (!hasYieldedLine && string.IsNullOrWhiteSpace(line))
                continue;

            hasYieldedLine = true;
            yield return line;
        }
    }
}
