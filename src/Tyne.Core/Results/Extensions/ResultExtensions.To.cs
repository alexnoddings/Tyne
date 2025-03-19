namespace Tyne;

public static partial class ResultExtensions
{
    public static ValueTask<Result<T, E>> ToValueTask<T, E>(this Result<T, E> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return ValueTask.FromResult(result);
    }

    public static Task<Result<T, E>> ToTask<T, E>(this Result<T, E> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return Task.FromResult(result);
    }

    public static Option<T> ToOption<T, E>(this Result<T, E> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (result.TryUnwrap(out var value, out _))
            return Option.Some(value);

        return Option.None<T>();
    }
}
