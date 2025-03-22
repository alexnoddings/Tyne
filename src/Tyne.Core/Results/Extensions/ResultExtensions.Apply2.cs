namespace Tyne;

// S4136:  Method overloads should be grouped together.
// REASON: Apply/ApplyImpl pairs are grouped together for ease of reading.
#pragma warning disable S4136
// All Apply methods which support Async have a Synchronous Apply method, which returns an Asynchronous ApplyImpl method.
// This is done so ArgumentNullExceptions are thrown sync when the method is called, rather than async when the returned Task is awaited.
// Earlier exceptions make more sense, and are better for diagnostics.
public static partial class ResultExtensions
{
    public static Result<T, TE> Apply<T, TE>(this Result<T, TE> result, Action<T> ok, Action<TE> error)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ok);
        ArgumentNullException.ThrowIfNull(error);

        if (result.TryUnwrap(out var value, out var errorValue))
            ok(value);
        else
            error(errorValue);

        return result;
    }

    public static Task<Result<T, TE>> Apply<T, TE>(this Task<Result<T, TE>> resultTask, Action<T> ok, Action<TE> error)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ok);
        ArgumentNullException.ThrowIfNull(error);

        return ApplyImpl(resultTask, ok, error);
    }

    private static async Task<Result<T, TE>> ApplyImpl<T, TE>(Task<Result<T, TE>> resultTask, Action<T> ok, Action<TE> error)
    {
        var result = await resultTask.ConfigureAwait(false);
        if (result.TryUnwrap(out var value, out var errorValue))
            ok(value);
        else
            error(errorValue);

        return result;
    }

    public static Task<Result<T, TE>> Apply<T, TE>(this Result<T, TE> result, Func<T, Task> ok, Func<TE, Task> error)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ok);
        ArgumentNullException.ThrowIfNull(error);

        if (result.TryUnwrap(out var value, out var errorValue))
            return ApplyImplOk(result, ok, value);

        return ApplyImplError(result, error, errorValue);
    }

    private static async Task<Result<T, TE>> ApplyImplError<T, TE>(Result<T, TE> result, Func<TE, Task> error, TE errorValue)
    {
        await error(errorValue).ConfigureAwait(false);

        return result;
    }

    public static Task<Result<T, TE>> Apply<T, TE>(this Task<Result<T, TE>> resultTask, Func<T, Task> ok, Func<TE, Task> error)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ok);
        ArgumentNullException.ThrowIfNull(error);

        return ApplyImpl(resultTask, ok, error);
    }

    private static async Task<Result<T, TE>> ApplyImpl<T, TE>(Task<Result<T, TE>> resultTask, Func<T, Task> ok, Func<TE, Task> error)
    {
        var result = await resultTask.ConfigureAwait(false);
        if (result.TryUnwrap(out var value, out var errorValue))
            await ok(value).ConfigureAwait(false);
        else
            await error(errorValue).ConfigureAwait(false);

        return result;
    }
}
