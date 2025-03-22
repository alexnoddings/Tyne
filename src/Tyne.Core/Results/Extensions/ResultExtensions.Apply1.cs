namespace Tyne;

// S4136:  Method overloads should be grouped together.
// REASON: Apply/ApplyImpl pairs are grouped together for ease of reading.
#pragma warning disable S4136
// All Apply methods which support Async have a Synchronous Apply method, which returns an Asynchronous ApplyImpl method.
// This is done so ArgumentNullExceptions are thrown sync when the method is called, rather than async when the returned Task is awaited.
// Earlier exceptions make more sense, and are better for diagnostics.
public static partial class ResultExtensions
{
    public static Result<T, TE> Apply<T, TE>(this Result<T, TE> result, Action<T> ok)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ok);

        if (result.TryUnwrap(out var value, out _))
            ok(value);

        return result;
    }

    public static Task<Result<T, TE>> Apply<T, TE>(this Task<Result<T, TE>> resultTask, Action<T> ok)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ok);

        return ApplyImpl(resultTask, ok);
    }

    private static async Task<Result<T, TE>> ApplyImpl<T, TE>(Task<Result<T, TE>> resultTask, Action<T> ok)
    {
        var result = await resultTask.ConfigureAwait(false);
        if (result.TryUnwrap(out var value, out _))
            ok(value);

        return result;
    }

    public static Task<Result<T, TE>> Apply<T, TE>(this Result<T, TE> result, Func<T, Task> ok)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ok);

        if (result.TryUnwrap(out var value, out _))
            return ApplyImplOk(result, ok, value);

        return result.ToTask();
    }

    private static async Task<Result<T, TE>> ApplyImplOk<T, TE>(Result<T, TE> result, Func<T, Task> ok, T okValue)
    {
        await ok(okValue).ConfigureAwait(false);

        return result;
    }

    public static Task<Result<T, TE>> Apply<T, TE>(this Task<Result<T, TE>> resultTask, Func<T, Task> ok)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ok);

        return ApplyImpl(resultTask, ok);
    }

    private static async Task<Result<T, TE>> ApplyImpl<T, TE>(Task<Result<T, TE>> resultTask, Func<T, Task> ok)
    {
        var result = await resultTask.ConfigureAwait(false);
        if (result.TryUnwrap(out var value, out _))
            await ok(value).ConfigureAwait(false);

        return result;
    }
}
