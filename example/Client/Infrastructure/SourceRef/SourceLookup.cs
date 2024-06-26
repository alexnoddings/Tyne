using System.Reflection;
using Tyne.SourceRef;

namespace Tyne.Aerospace.Client.Infrastructure.SourceRef;

internal static class SourceLookup
{
    private enum LookupVariant
    {
        Source,
        Path
    }

    private const BindingFlags LookupBindingFlags = BindingFlags.Public | BindingFlags.Static;

    public static string Source<T>(SourceCodeType sourceCodeType) =>
        Source(sourceCodeType, typeof(T));

    public static string Source(SourceCodeType sourceCodeType, Type targetType) =>
        LookupTargetType(GetLookupType(sourceCodeType, LookupVariant.Source), targetType);

    public static string Path<T>(SourceCodeType sourceCodeType) =>
        Path(sourceCodeType, typeof(T));

    public static string Path(SourceCodeType sourceCodeType, Type targetType) =>
        LookupTargetType(GetLookupType(sourceCodeType, LookupVariant.Path), targetType);

    // Needs to use the same identifier normalisation logic as SourceRef.SourceGenerators
    private static string NormaliseTypeIdentifier(Type type) =>
        (type.FullName ?? type.Name)
        .Replace("_", "__", StringComparison.Ordinal)
        .Replace(".", "_", StringComparison.Ordinal)
        .Replace("`", "_", StringComparison.Ordinal);

    private static Type GetLookupType(SourceCodeType sourceCodeType, LookupVariant variant) =>
        (sourceCodeType, variant) switch
        {
            // These types are generated by the SourceRef.SourceGenerators
            (SourceCodeType.Component, LookupVariant.Source) => typeof(SourceRefComponentSources),
            (SourceCodeType.Component, LookupVariant.Path) => typeof(SourceRefComponentPaths),
            (SourceCodeType.Type, LookupVariant.Source) => typeof(SourceRefTypeSources),
            (SourceCodeType.Type, LookupVariant.Path) => typeof(SourceRefTypePaths),
            _ => throw new InvalidOperationException("Invalid lookup operation."),
        };

    private static string LookupTargetType(Type lookupType, Type targetType)
    {
        // Generic type names are ugly (e.g. Namespace.Type`1[[Namespace.GenericArg, AssemblyName, AssemblyVersion, Culture, PublicKeyToken]]),
        // so we deconstruct it back to it's definition
        if (targetType.IsGenericType)
            targetType = targetType.GetGenericTypeDefinition();

        var typeIdentifier = NormaliseTypeIdentifier(targetType);
        var lookupField = lookupType.GetField(typeIdentifier, LookupBindingFlags)
            ?? throw new ArgumentException($"Target type \"{targetType.Name}\" does not have a lookup on lookup type \"{lookupType.Name}\".");
        var value = lookupField.GetValue(null);
        if (value is not string valueStr)
            throw new InvalidOperationException("Lookup field value is not a valid string.");

        return valueStr;
    }
}
