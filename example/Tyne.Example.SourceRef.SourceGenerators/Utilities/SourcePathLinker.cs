namespace Tyne.SourceRef.SourceGenerators.Utilities;

internal static class SourcePathLinker
{
    private static bool _isInitialised;
    private static string? _gitUrlBase;

    private static void EnsureInitialised()
    {
        if (_isInitialised)
            return;

        _isInitialised = true;

        var serverUrl = Environment.GetEnvironmentVariable("GITHUB_SERVER_URL");
        if (string.IsNullOrWhiteSpace(serverUrl))
            return;

        var repoName = Environment.GetEnvironmentVariable("GITHUB_REPOSITORY");
        if (string.IsNullOrWhiteSpace(repoName))
            return;

        var commitSha = Environment.GetEnvironmentVariable("GITHUB_SHA");
        if (string.IsNullOrWhiteSpace(commitSha))
            return;

        _gitUrlBase = $"{serverUrl}/{repoName}/tree/{commitSha}";
    }

    /// <summary>
    ///     Generates an absolute link to the file at <paramref name="absolutePath"/>.
    /// </summary>
    /// <remarks>
    ///     When executing in a GitHub action, this generates a link to the source on GitHub.
    ///     Otherwise, it falls back to generating a file:// link
    /// </remarks>
    public static string Link(string absolutePath, string solutionRootPath)
    {
        EnsureInitialised();

        if (_gitUrlBase is not null)
        {
            if (!absolutePath.StartsWith(solutionRootPath, StringComparison.Ordinal))
                throw new ArgumentException($"Path \"{absolutePath}\" is not in the solution.", nameof(absolutePath));

            var pathRelativeToSolution = absolutePath.Substring(solutionRootPath.Length);
            return _gitUrlBase + pathRelativeToSolution;
        }

        // Browsers won't open file:// links normally (it's a security risk),
        // this is just to prove the system is working when dev-ing locally
        return "file://" + absolutePath;
    }
}
