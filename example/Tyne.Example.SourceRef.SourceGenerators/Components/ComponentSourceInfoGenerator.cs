using Microsoft.CodeAnalysis;

namespace Tyne.SourceRef.SourceGenerators.Components;

[Generator]
public class ComponentSourceInfoGenerator : BaseSourceRefGenerator
{
    protected override string GeneratorTypeName => "Component";

    protected override ICollection<SourceInfo> GetSources(GeneratorExecutionContext context) =>
        ComponentSourceInfoCollector.Collect(context.AdditionalFiles, RootNamespace, ProjectDirectory).ToList();
}
