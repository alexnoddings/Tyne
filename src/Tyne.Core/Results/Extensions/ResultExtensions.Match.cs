namespace Tyne;

public static partial class ResultExtensions
{
    public static TResult Match<T, E, TResult>(this Result<T, E> result, Func<T, TResult> ok, Func<E, TResult> error)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ok);
        ArgumentNullException.ThrowIfNull(error);

        if (result.TryUnwrap(out var value, out var errorValue))
            return ok(value);

        return error(errorValue);
    }

    public static Task MatchAsync<T, E>(this Result<T, E> result, Func<T, Task> ok, Func<E, Task> error) =>
        Match(result, ok, error);

    public static Task<TResult> MatchAsync<T, E, TResult>(this Result<T, E> result, Func<T, Task<TResult>> ok, Func<E, Task<TResult>> error) =>
        Match(result, ok, error);
}
