#if NET9_0_OR_GREATER
using System.Buffers;
#endif
using System.Runtime.CompilerServices;
using System.Text;

namespace Tyne.EntityFramework;

internal static class DbContextChangeAuditorTypeNameFormatter
{
#if NET9_0_OR_GREATER
    private static readonly SearchValues<string> IgnoredNamespace = SearchValues.Create(["System", "System.Collections.Generic"], StringComparison.OrdinalIgnoreCase);
#else
    private static readonly string[] IgnoredNamespaces = ["System", "System.Collections.Generic"];
#endif

    internal static string FormatTypeName(Type type)
    {
        var typeIdentifierBuilder = new StringBuilder();
        AppendTypeIdentifier(type);
        return typeIdentifierBuilder.ToString();

        void AppendTypeIdentifier(Type type)
        {
            // Non-generic types (e.g. int, string) can just be ToString-ed
            // (produces Name.Space.Type)
            var typeFullName = type.ToString();
            var typeNamespace = type.Namespace;

            if (typeNamespace is not null && ShouldIgnoreNamespace(typeNamespace))
            {
                var namespaceEndPos = typeNamespace.Length + 1;
                typeFullName = typeFullName[namespaceEndPos ..];
            }

            if (!type.IsGenericType)
            {
                _ = typeIdentifierBuilder.Append(typeFullName);
                return;
            }

            // Nullable is a special case
            if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var innerType = type.GetGenericArguments()[0];

                // Checks against open generic Nullable<> rather than closed Nullable<T>
                if (innerType.IsGenericTypeParameter)
                    _ = typeIdentifierBuilder.Append('T');
                else
                    AppendTypeIdentifier(innerType);

                _ = typeIdentifierBuilder.Append('?');
                return;
            }

            // Trim everything after `
            // e.g. Some.Name.Space`2[Some.Other.Type,System.Nullable`1[System.Guid]]
            //   -> Some.Name.Space
            // then we can handle adding the generic info afterwards ourselves
            var typeGenericStart = typeFullName.IndexOf('`', StringComparison.Ordinal);
            var typeIdentifier = typeFullName[..typeGenericStart];
            _ = typeIdentifierBuilder.Append(typeIdentifier).Append('<');

            foreach (var (index, genericTypeArgument) in type.GetGenericArguments().Index())
            {
                if (index > 0)
                    _ = typeIdentifierBuilder.Append(", ");

                if (genericTypeArgument.IsGenericTypeParameter)
                {
                    // The type isn't closed (e.g. we have List<> rather than List<int>)
                    // So this argument is T rather than System.Int32,
                    // which means we can't meaningfully reflect any further
                    _ = typeIdentifierBuilder.Append(genericTypeArgument.Name);
                    continue;
                }

                AppendTypeIdentifier(genericTypeArgument);
            }

            _ = typeIdentifierBuilder.Append('>');
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool ShouldIgnoreNamespace(string typeNamespace)
    {
        if (string.IsNullOrEmpty(typeNamespace))
            return false;

#if NET9_0_OR_GREATER
        if (IgnoredNamespace.Contains(typeNamespace))
            return true;
#else
        if (Array.Exists(IgnoredNamespaces, ignoredNamespace => typeNamespace.Equals(ignoredNamespace, StringComparison.Ordinal)))
            return true;
#endif

        return false;
    }

#if !NET9_0_OR_GREATER
    // Implements .NET 9's .Index() method
    // Adapted from https://github.com/dotnet/runtime/blob/1d1bf92fcf43aa6981804dc53c5174445069c9e4/src/libraries/System.Linq/src/System/Linq/Index.cs#L10-L41
    private static IEnumerable<(int Index, Type Item)> Index(this Type[] source)
    {
        ArgumentNullException.ThrowIfNull(source);
        return IndexIterator(source);
    }

    private static IEnumerable<(int Index, Type Item)> IndexIterator(Type[] source)
    {
        var index = -1;
        foreach (var element in source)
        {
            index++;
            yield return (index, element);
        }
    }
#endif
}
