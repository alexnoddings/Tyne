namespace Tyne;

public static partial class OptionExtensions
{
    public static Option<T> Apply<T>(this Option<T> option, Action<T> some)
    {
        ArgumentNullException.ThrowIfNull(some);

        if (option.TryUnwrap(out var value))
            some(value);

        return option;
    }
}
