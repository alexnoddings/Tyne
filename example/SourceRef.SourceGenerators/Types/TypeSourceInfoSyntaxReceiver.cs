using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;

namespace Tyne.SourceRef.SourceGenerators.Types;

// Visits type declarations and creates SourceInfos
internal sealed class TypeSourceInfoSyntaxReceiver : ISyntaxReceiver
{
    public List<SourceInfo> TypeSources { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not TypeDeclarationSyntax typeDeclaration)
            return;

        var typeNamespace = GetContainingIdentifier(typeDeclaration);
        if (typeNamespace is null)
            return;

        // Build the type's full identifier
        var identifier = typeNamespace + "." + typeDeclaration.Identifier.ValueText;

        if (typeDeclaration.Arity > 0)
            identifier = $"{identifier}`{typeDeclaration.Arity}";

        // Get the absolute path to the type's definition
        var absolutePath =
            syntaxNode.GetLocation().SourceTree?.FilePath
            ?? throw new InvalidOperationException("Could not determine file path.");

        // Get the type's source (loading from the syntax tree skips any surrounding nodes/trivia such as using directives)
        var typeSource =
            syntaxNode.SyntaxTree
            .ToString()
            .Substring(syntaxNode.FullSpan.Start, syntaxNode.FullSpan.Length);

        // Format the source
        var typeSourceFormatted = TypeSourceFormatter.Format(typeSource);

        TypeSources.Add(new SourceInfo(identifier, absolutePath, typeSourceFormatted));
    }

    /// <summary>
    ///     Recursively builds the identifier of a node.
    /// </summary>
    private static string? GetContainingIdentifier(SyntaxNode? node) =>
        node?.Parent switch
        {
            // If this node is defined in a namespace, we've found the root of the identifier
            BaseNamespaceDeclarationSyntax namespaceDeclaration => namespaceDeclaration.Name.ToString(),
            // If this node is defined in a type (e.g. nested type), figure out the identifier of that type
            TypeDeclarationSyntax typeDeclaration => GetTypeIdentifier(typeDeclaration),
            // If this is some other node, keep walking back up the tree
            SyntaxNode other => GetContainingIdentifier(other),
            _ => null
        };

    private static string? GetTypeIdentifier(TypeDeclarationSyntax typeDeclaration)
    {
        var classIdentifier = typeDeclaration.Identifier.ValueText;
        // Get the identifier containing this type
        var prefixIdentifier = GetContainingIdentifier(typeDeclaration);

        return prefixIdentifier + "." + classIdentifier;
    }
}
