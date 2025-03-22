namespace Tyne;

// S4136:  Method overloads should be grouped together.
// REASON: Match/MatchImpl pairs are grouped together for ease of reading.
#pragma warning disable S4136
// All Match methods which support Async have a Synchronous Match method, which returns an Asynchronous MatchImpl method.
// This is done so ArgumentNullExceptions are thrown sync when the method is called, rather than async when the returned Task is awaited.
// Earlier exceptions make more sense, and are better for diagnostics.
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

    public static Task<TResult> Match<T, TE, TResult>(this Result<T, TE> result, Func<T, Task<TResult>> ok, Func<TE, Task<TResult>> error)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ok);
        ArgumentNullException.ThrowIfNull(error);

        return MatchImpl(result, ok, error);
    }

    private static async Task<TResult> MatchImpl<T, TE, TResult>(Result<T, TE> result, Func<T, Task<TResult>> ok, Func<TE, Task<TResult>> error)
    {
        if (result.TryUnwrap(out var value, out var errorValue))
            return await ok(value).ConfigureAwait(false);

        return await error(errorValue).ConfigureAwait(false);
    }

    public static Task<TResult> Match<T, TE, TResult>(this Task<Result<T, TE>> resultTask, Func<T, TResult> ok, Func<TE, TResult> error)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ok);
        ArgumentNullException.ThrowIfNull(error);

        return MatchImpl(resultTask, ok, error);
    }

    private static async Task<TResult> MatchImpl<T, TE, TResult>(Task<Result<T, TE>> resultTask, Func<T, TResult> ok, Func<TE, TResult> error)
    {
        var result = await resultTask.ConfigureAwait(false);
        if (result.TryUnwrap(out var value, out var errorValue))
            return ok(value);

        return error(errorValue);
    }

    public static Task<TResult> Match<T, TE, TResult>(this Task<Result<T, TE>> resultTask, Func<T, Task<TResult>> ok, Func<TE, Task<TResult>> error)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ok);
        ArgumentNullException.ThrowIfNull(error);

        return MatchImpl(resultTask, ok, error);
    }

    private static async Task<TResult> MatchImpl<T, TE, TResult>(Task<Result<T, TE>> resultTask, Func<T, Task<TResult>> ok, Func<TE, Task<TResult>> error)
    {
        var result = await resultTask.ConfigureAwait(false);
        if (result.TryUnwrap(out var value, out var errorValue))
            return await ok(value).ConfigureAwait(false);

        return await error(errorValue).ConfigureAwait(false);
    }
}
