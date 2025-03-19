using System.Diagnostics.CodeAnalysis;

namespace Tyne.Utilities;

internal static class TyneToStringHelpers
{
    public static string CreateStringFor<TValue>(string prefix, [DisallowNull] TValue value)
    {
        var inner = value.ToString();
        var stringLength = prefix.Length + "()".Length + (inner?.Length ?? 0);
        return string.Create(
            null,
            stackalloc char[stringLength],
            $"{prefix}({inner})"
        );
    }
}
