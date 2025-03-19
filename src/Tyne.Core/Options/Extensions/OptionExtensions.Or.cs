using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Tyne;

public static partial class OptionExtensions
{
    [Pure]
    [return: NotNullIfNotNull(nameof(value))]
    public static T Or<T>(this Option<T> option, T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (option.TryUnwrap(out var optionValue))
            return optionValue;

        return value;
    }

    public static T Or<T>(this Option<T> option, Func<T> valueFactory)
    {
        ArgumentNullException.ThrowIfNull(valueFactory);

        if (option.TryUnwrap(out var optionValue))
            return optionValue;

        return valueFactory();
    }

    [Pure]
    public static T? OrDefault<T>(this Option<T> option)
    {
        if (option.TryUnwrap(out var optionValue))
            return optionValue;

        return default;
    }

    [Pure]
    public static T? OrNull<T>(this Option<T> option) where T : struct
    {
        if (option.TryUnwrap(out var optionValue))
            return optionValue;

        return null;
    }
}
