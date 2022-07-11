using ColorCode;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Tyne.Docs.SourceGen;

[Generator]
public class SampleSourceGenerator : ISourceGenerator
{
	public const string SourceCodeFieldName = "SourceCode";
	public const string SourceCodeHtmlFieldName = "SourceCodeHtml";

	private static DiagnosticDescriptor SampleClassNotPartialDiagnostic { get; } = 
		new DiagnosticDescriptor(id: "TYNEDOCGEN01", title: "Sample is not partial.", messageFormat: "The class '{0}' marked as a [Sample] is not partial.", category: "TyneDocGen", DiagnosticSeverity.Warning, isEnabledByDefault: true);

	private static DiagnosticDescriptor CouldNotLoadNamespaceDiagnostic { get; } =
		new DiagnosticDescriptor(id: "TYNEDOCGEN02", title: "Failed to find sample namespace.", messageFormat: "Could not load namespace for class '{0}' marked as [Sample].", category: "TyneDocGen", DiagnosticSeverity.Warning, isEnabledByDefault: true);

	public void Initialize(GeneratorInitializationContext context)
	{
		context.RegisterForSyntaxNotifications(() => new SampleCodeSyntaxReceiver());
	}

	public void Execute(GeneratorExecutionContext context)
	{
		var htmlFormatter = new HtmlClassFormatter();
		var syntaxReceiver = (SampleCodeSyntaxReceiver)context.SyntaxReceiver!;

		foreach (var classDeclaration in syntaxReceiver.SampleCodeClasses)
		{
			var classIdentifier = classDeclaration.Identifier.ValueText;

			var isPartial = classDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword);
			if (!isPartial)
			{
				context.ReportDiagnostic(Diagnostic.Create(SampleClassNotPartialDiagnostic, classDeclaration.GetLocation(), classIdentifier));
				continue;
			}

			var semanticModel = context.Compilation.GetSemanticModel(classDeclaration.SyntaxTree);
			var syntaxTreeRoot = semanticModel.SyntaxTree.GetRoot() as CompilationUnitSyntax;
			var namespaceDeclaration = syntaxTreeRoot!.Members.OfType<BaseNamespaceDeclarationSyntax>().FirstOrDefault();
			if (namespaceDeclaration is null)
			{
				context.ReportDiagnostic(Diagnostic.Create(CouldNotLoadNamespaceDiagnostic, classDeclaration.GetLocation(), classIdentifier));
				continue;
			}

			var fileNamespace = namespaceDeclaration.Name.ToString();
			var classSourceCode = classDeclaration.ToString();
			classSourceCode = classSourceCode.Replace("[Sample]\r\npublic static partial class", "public static class").Replace("\"", "\"\"");

			var classSourceCodeAsHtml = htmlFormatter.GetHtmlString(classSourceCode.Replace("\t", "   "), Languages.CSharp).Replace("\"", "\"\"");

			var generatedSourceTextStr =
$@"namespace {fileNamespace};

public static partial class {classIdentifier}
{{
public static readonly string {SourceCodeFieldName} =
@""{classSourceCode}"";

public static readonly string {SourceCodeHtmlFieldName} =
@""{classSourceCodeAsHtml}"";
}}";

			var generatedSourceText = SourceText.From(generatedSourceTextStr, Encoding.UTF8);
			context.AddSource($"{fileNamespace}.{classDeclaration.Identifier.Value}.src.g.cs", generatedSourceText);
		}
	}

	private sealed class SampleCodeSyntaxReceiver : ISyntaxReceiver
	{
		public List<ClassDeclarationSyntax> SampleCodeClasses { get; } = new();

		public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
		{
			if (syntaxNode is not ClassDeclarationSyntax classDeclarationSyntax)
				return;

			var attributeSyntaxes = classDeclarationSyntax.AttributeLists.SelectMany(attributeList => attributeList.Attributes);
			if (attributeSyntaxes.Any(attributeSyntax => attributeSyntax.Name is IdentifierNameSyntax identifier && identifier.Identifier.ValueText == "Sample"))
				SampleCodeClasses.Add(classDeclarationSyntax);
		}
	}
}
