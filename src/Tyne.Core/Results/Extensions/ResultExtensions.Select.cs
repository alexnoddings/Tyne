namespace Tyne;

// S4136:  Method overloads should be grouped together.
// REASON: Select/SelectImpl pairs are grouped together for ease of reading.
#pragma warning disable S4136
// All Select methods which support have a Synchronous Select method, which returns an Asynchronous SelectImpl method.
// This is done so ArgumentNullExceptions are thrown sync when the method is called, rather than async when the returned Task is awaited.
// Earlier exceptions make more sense, and are better for diagnostics.
public static partial class ResultExtensions
{
    public static Result<TResult, E> Select<T, E, TResult>(this Result<T, E> result, Func<T, TResult> selector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(selector);

        if (!result.TryUnwrap(out var value, out var errorValue))
            return Result.Error<TResult, E>(errorValue);

        var newValue = selector(value);
        if (newValue is null)
            throw new ArgumentException("Selector returned a null value.", nameof(selector));

        return Result.Ok<TResult, E>(newValue);
    }

    public static Task<Result<TResult, E>> Select<T, E, TResult>(this Result<T, E> result, Func<T, Task<TResult>> selector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(selector);

        return SelectImpl(result, selector);
    }

    private static async Task<Result<TResult, E>> SelectImpl<T, E, TResult>(Result<T, E> result, Func<T, Task<TResult>> selector)
    {
        if (!result.TryUnwrap(out var value, out var errorValue))
            return Result.Error<TResult, E>(errorValue);

        var task = selector(value);
        if (task is null)
            throw new ArgumentException("Selector returned a null task.", nameof(selector));

        var newValue = await task.ConfigureAwait(false);
        if (newValue is null)
            throw new ArgumentException("Selector returned a null value.", nameof(selector));

        return Result.Ok<TResult, E>(newValue);
    }

    public static Task<Result<TResult, E>> Select<T, E, TResult>(this Task<Result<T, E>> resultTask, Func<T, TResult> selector)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(selector);

        return SelectImpl(resultTask, selector);
    }

    private static async Task<Result<TResult, E>> SelectImpl<T, E, TResult>(Task<Result<T, E>> resultTask, Func<T, TResult> selector)
    {
        var result = await resultTask.ConfigureAwait(false);
        if (!result.TryUnwrap(out var value, out var errorValue))
            return Result.Error<TResult, E>(errorValue);

        var newValue = selector(value);
        if (newValue is null)
            throw new ArgumentException("Selector returned a null value.", nameof(selector));

        return Result.Ok<TResult, E>(newValue);
    }

    public static Task<Result<TResult, E>> Select<T, E, TResult>(this Task<Result<T, E>> resultTask, Func<T, Task<TResult>> selector)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(selector);

        return SelectImpl(resultTask, selector);
    }

    private static async Task<Result<TResult, E>> SelectImpl<T, E, TResult>(Task<Result<T, E>> resultTask, Func<T, Task<TResult>> selector)
    {
        var result = await resultTask.ConfigureAwait(false);
        if (!result.TryUnwrap(out var value, out var errorValue))
            return Result.Error<TResult, E>(errorValue);

        var task = selector(value);
        if (task is null)
            throw new ArgumentException("Selector returned a null task.", nameof(selector));

        var newValue = await task.ConfigureAwait(false);
        if (newValue is null)
            throw new ArgumentException("Selector returned a null value.", nameof(selector));

        return Result.Ok<TResult, E>(newValue);
    }
}
#pragma warning restore S4136
