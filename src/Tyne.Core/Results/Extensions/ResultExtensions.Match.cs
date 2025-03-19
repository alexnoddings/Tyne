namespace Tyne;

public static partial class ResultExtensions
{
    public static TResult Match<T, TE, TResult>(this Result<T, TE> result, Func<T, TResult> ok, Func<TE, TResult> error)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ok);
        ArgumentNullException.ThrowIfNull(error);

        if (result.TryUnwrap(out var value, out var errorValue))
            return ok(value);

        return error(errorValue);
    }

    public static Task MatchAsync<T, TE>(this Result<T, TE> result, Func<T, Task> ok, Func<TE, Task> error) =>
        Match(result, ok, error);

    public static Task<TResult> MatchAsync<T, TE, TResult>(this Result<T, TE> result, Func<T, Task<TResult>> ok, Func<TE, Task<TResult>> error) =>
        Match(result, ok, error);
}
