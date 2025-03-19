namespace Tyne;

// S4136:  Method overloads should be grouped together.
// REASON: Select/SelectImpl pairs are grouped together for ease of reading.
#pragma warning disable S4136
// All Select methods which support have a Synchronous Select method, which returns an Asynchronous SelectImpl method.
// This is done so ArgumentNullExceptions are thrown sync when the method is called, rather than async when the returned Task is awaited.
// Earlier exceptions make more sense, and are better for diagnostics.
public static partial class OptionExtensions
{
    public static Option<TResult> Select<T, TResult>(this Option<T> option, Func<T, TResult> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        if (!option.TryUnwrap(out var value))
            return Option.None<TResult>();

        var newValue = selector(value);
        if (newValue is null)
            return Option.None<TResult>();

        return Option.Some<TResult>(newValue);
    }

    public static Task<Option<TResult>> Select<T, TResult>(this Option<T> option, Func<T, Task<TResult>> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        return SelectImpl(option, selector);
    }

    private static async Task<Option<TResult>> SelectImpl<T, TResult>(Option<T> option, Func<T, Task<TResult>> selector)
    {
        if (!option.TryUnwrap(out var value))
            return Option.None<TResult>();

        var task = selector(value);
        if (task is null)
            return Option.None<TResult>();

        var newValue = await task.ConfigureAwait(false);
        if (newValue is null)
            return Option.None<TResult>();

        return Option.Some<TResult>(newValue);
    }

    public static Task<Option<TResult>> Select<T, TResult>(this Task<Option<T>> optionTask, Func<T, TResult> selector)
    {
        ArgumentNullException.ThrowIfNull(optionTask);
        ArgumentNullException.ThrowIfNull(selector);

        return SelectImpl(optionTask, selector);
    }

    private static async Task<Option<TResult>> SelectImpl<T, TResult>(Task<Option<T>> optionTask, Func<T, TResult> selector)
    {
        var option = await optionTask.ConfigureAwait(false);
        if (!option.TryUnwrap(out var value))
            return Option.None<TResult>();

        var newValue = selector(value);
        if (newValue is null)
            return Option.None<TResult>();

        return Option.Some<TResult>(newValue);
    }

    public static Task<Option<TResult>> Select<T, TResult>(this Task<Option<T>> optionTask, Func<T, Task<TResult>> selector)
    {
        ArgumentNullException.ThrowIfNull(optionTask);
        ArgumentNullException.ThrowIfNull(selector);

        return SelectImpl(optionTask, selector);
    }

    private static async Task<Option<TResult>> SelectImpl<T, TResult>(Task<Option<T>> optionTask, Func<T, Task<TResult>> selector)
    {
        var option = await optionTask.ConfigureAwait(false);
        if (!option.TryUnwrap(out var value))
            return Option.None<TResult>();

        var task = selector(value);
        if (task is null)
            return Option.None<TResult>();

        var newValue = await task.ConfigureAwait(false);
        if (newValue is null)
            return Option.None<TResult>();

        return Option.Some<TResult>(newValue);
    }
}
#pragma warning restore S4136
