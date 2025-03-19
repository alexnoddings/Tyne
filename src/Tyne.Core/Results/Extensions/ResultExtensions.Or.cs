using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Tyne;

public static partial class ResultExtensions
{
    [Pure]
    [return: NotNullIfNotNull(nameof(value))]
    public static T Or<T, E>(this Result<T, E> result, T value)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(value);

        if (result.TryUnwrap(out var resultValue, out _))
            return resultValue;

        return value;
    }

    public static T Or<T, E>(this Result<T, E> result, Func<T> valueFactory)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(valueFactory);

        if (result.TryUnwrap(out var resultValue, out _))
            return resultValue;

        return valueFactory();
    }

    [Pure]
    public static T? OrDefault<T, E>(this Result<T, E> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (result.TryUnwrap(out var resultValue, out _))
            return resultValue;

        return default;
    }

    [Pure]
    public static T? OrNull<T, E>(this Result<T, E> result) where T : struct
    {
        ArgumentNullException.ThrowIfNull(result);

        if (result.TryUnwrap(out var resultValue, out _))
            return resultValue;

        return null;
    }
}
