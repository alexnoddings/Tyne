using Microsoft.CodeAnalysis;
using Tyne.SourceRef.SourceGenerators.Utilities;

namespace Tyne.SourceRef.SourceGenerators;

public abstract class BaseSourceRefGenerator : ISourceGenerator
{
    private string? _projectDirectory;
    protected string ProjectDirectory => EnsureConfigInitialised(_projectDirectory);

    private string? _solutionDirectory;
    protected string SolutionDirectory => EnsureConfigInitialised(_solutionDirectory);

    private string? _rootNamespace;
    protected string RootNamespace => EnsureConfigInitialised(_rootNamespace);

    public virtual void Initialize(GeneratorInitializationContext context)
    {
    }

    // Throws if a config string is not set
    private static string EnsureConfigInitialised(string? config) =>
        config ?? throw new InvalidOperationException("Config is not initialised.");

    // Initialises a string field based on the context
    private static void InitialiseConfigField(GeneratorExecutionContext context, string key, ref string? field)
    {
        if (context.AnalyzerConfigOptions.GlobalOptions.TryGetValue(key, out field))
            return;

        throw new InvalidOperationException($"Generator context did not have an expected \"{key}\" build property.");
    }

    protected abstract string GeneratorTypeName { get; }
    protected abstract ICollection<SourceInfo> GetSources(GeneratorExecutionContext context);

    public void Execute(GeneratorExecutionContext context)
    {
        InitialiseConfigField(context, "build_property.projectdir", ref _projectDirectory);
        InitialiseConfigField(context, "build_property.rootnamespace", ref _rootNamespace);
        _solutionDirectory = GetSolutionRootPath(_projectDirectory!);

        var sources = GetSources(context);

        AddClass(context, "Paths", sources, CreateSourcePathField);
        AddClass(context, "Sources", sources, CreateSourceContentsField);
    }

    private void AddClass(GeneratorExecutionContext context, string classType, IEnumerable<SourceInfo> sources, Func<SourceInfo, string> fieldFunc)
    {
        var className = $"SourceRef{GeneratorTypeName}{classType}";
        var fileName = className + ".g.cs";

        var classSource = ClassGenerator.GenerateClass(className, sources, fieldFunc);
        context.AddSource(fileName, classSource);
    }

    private string CreateSourcePathField(SourceInfo sourceInfo) =>
        SourcePathLinker.Link(sourceInfo.AbsolutePath, SolutionDirectory).Replace("\\", "\\\\");

    private static string CreateSourceContentsField(SourceInfo sourceInfo) =>
        sourceInfo.Contents;

    // Identifies the root of the solution
    private const string SolutionFileName = "Tyne.sln";

    // Given a path in the solution, it will walk backwards until it finds the root of the solution
    private static string GetSolutionRootPath(string path)
    {
        var solutionFilePath = Path.Combine(path, SolutionFileName);
        if (File.Exists(solutionFilePath))
            return path;

        // Already at the path root, we can't walk back any further
        if (Path.GetPathRoot(path) == path)
            throw new InvalidOperationException($"Could not find solution root file \"{SolutionFileName}\".");

        var parentPath = Directory.GetParent(path).FullName;
        return GetSolutionRootPath(parentPath);
    }

    // Makes a path relative to the root of the solution
    protected string GetPathRelativeToSolution(string absolutePath)
    {
        if (absolutePath is null)
            throw new ArgumentNullException(nameof(absolutePath));

        EnsureConfigInitialised(_solutionDirectory);

        if (!absolutePath.StartsWith(_solutionDirectory, StringComparison.Ordinal))
            throw new ArgumentException($"Path \"{absolutePath}\" is not in the solution.", nameof(absolutePath));

        return absolutePath.Substring(_solutionDirectory!.Length);
    }
}
