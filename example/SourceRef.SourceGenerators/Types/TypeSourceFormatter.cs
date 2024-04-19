namespace Tyne.SourceRef.SourceGenerators.Types;

internal static class TypeSourceFormatter
{
    private static readonly string[] FileContentsSplit = ["\n"];

    /// <summary>
    ///     Formats the source code of a type.
    /// </summary>
    public static string Format(string contents)
    {
        // First split the source by newline, keep the empty entries
        var split = contents.Replace("\r\n", "\n").Split(FileContentsSplit, StringSplitOptions.None);

        // Calculate the minimum indentation - we use this do outdent the source
        // This is especially useful for nested types to avoid the source all being left-padded
        var minIndent = split.Select(FirstIndexOfNonWhitespace).Where(index => index >= 0).Min();

        var formatted =
            split
            // Strip the indent from the front of each line
            // If the line is shorter than the minimum indent, it must be whitespace
            // Otherwise, trip the initial indent
            .Select(line => line.Length <= minIndent
                            ? string.Empty
                            : line.Substring(minIndent)
            )
            // Escape '\'s
            .Select(line => line.Replace("\\", "\\\\"))
            // Escape '"'s
            .Select(line => line.Replace("\"", "\\\""));

        // Joins by escaped newlines, and trim any excess whitespace from the end
        return string.Join("\\n", formatted).TrimEnd();
    }

    /// <summary>
    ///     Calculates the first index of a non-whitespace character in <paramref name="str"/>, or -1 if one is not found.
    /// </summary>
    private static int FirstIndexOfNonWhitespace(string str)
    {
        for (var i = 0; i < str.Length; i++)
        {
            // This only runs on individual lines, so we only check spaces and tabs
            if (str[i] is not ' ' and not '\t')
                return i;
        }

        return -1;
    }
}
