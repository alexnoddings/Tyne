using Microsoft.CodeAnalysis;

namespace Tyne.SourceRef.SourceGenerators.Types;

[Generator]
public class TypeSourceInfoGenerator : BaseSourceRefGenerator
{
    protected override string GeneratorTypeName => "Type";

    public override void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new TypeSourceInfoSyntaxReceiver());
    }

    protected override ICollection<SourceInfo> GetSources(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not TypeSourceInfoSyntaxReceiver syntaxReceiver)
            throw new InvalidOperationException("Invalid syntax receiver.");

        return syntaxReceiver.TypeSources;
    }
}
