namespace Tyne.SourceRef.SourceGenerators;

/// <summary>
///     Describes the source of a definition (e.g. type or component).
/// </summary>
public class SourceInfo
{
    /// <summary>
    ///     The source's identifier.
    /// </summary>
    /// <remarks>
    ///     This is the full namespace and class name.
    /// </remarks>
    public string Identifier { get; }

    /// <summary>
    ///     A normalised version of <see cref="Identifier"/> suitable for using in members.
    ///     This escapes _s, and replaces .s with _s.
    /// </summary>
    public string IdentifierNormalised => NormaliseIdentifier(Identifier);

    /// <summary>
    ///     The absolute path on disk to the source file.
    /// </summary>
    public string AbsolutePath { get; }

    /// <summary>
    ///     The contents of the source file.
    /// </summary>
    /// <remarks>
    ///     This is a trimmed/formatted version of the file found at <see cref="AbsolutePath"/>.
    /// </remarks>
    public string Contents { get; }

    /// <summary>
    ///     Creates a new <see cref="SourceInfo"/>.
    /// </summary>
    /// <param name="identifier">The <see cref="Identifier"/>.</param>
    /// <param name="absolutePath">The <see cref="AbsolutePath"/>.</param>
    /// <param name="contents">The <see cref="Contents"/>.</param>
    public SourceInfo(string identifier, string absolutePath, string contents)
    {
        Identifier = identifier;
        AbsolutePath = absolutePath;
        Contents = contents;
    }

    // Normalises a source identifier for use in member names
    // Escape _s with __s,
    // replace .s with _s,
    // replace `s with _s
    private static string NormaliseIdentifier(string identifier) =>
        identifier
        .Replace("_", "__")
        .Replace(".", "_")
        // Replacing both . and ` with _ shouldn't be an issue.
        // Normalisation is a one-way conversion (we never need to map back),
        // and the ` is only used to describe arity (so will only have a number directly succeeding it),
        // and type identifiers can't start with a digit.
        .Replace("`", "_");
}
