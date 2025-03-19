using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Tyne;

public static partial class ResultExtensions
{
    [Pure]
    [return: NotNullIfNotNull(nameof(value))]
    public static T Or<T, TE>(this Result<T, TE> result, T value)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(value);

        if (result.TryUnwrap(out var resultValue, out _))
            return resultValue;

        return value;
    }

    public static T Or<T, TE>(this Result<T, TE> result, Func<T> valueFactory)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(valueFactory);

        if (result.TryUnwrap(out var resultValue, out _))
            return resultValue;

        return valueFactory();
    }

    [Pure]
    public static T? OrDefault<T, TE>(this Result<T, TE> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (result.TryUnwrap(out var resultValue, out _))
            return resultValue;

        return default;
    }

    [Pure]
    public static T? OrNull<T, TE>(this Result<T, TE> result) where T : struct
    {
        ArgumentNullException.ThrowIfNull(result);

        if (result.TryUnwrap(out var resultValue, out _))
            return resultValue;

        return null;
    }
}
