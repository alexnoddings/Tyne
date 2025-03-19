namespace Tyne;

public static partial class OptionExtensions
{
    public static TResult Match<T, TResult>(this Option<T> option, Func<T, TResult> some, Func<TResult> none)
    {
        ArgumentNullException.ThrowIfNull(some);
        ArgumentNullException.ThrowIfNull(none);

        if (option.TryUnwrap(out var value))
            return some(value);

        return none();
    }

    public static Task MatchAsync<T>(this Option<T> option, Func<T, Task> some, Func<Task> none) =>
        Match(option, some, none);

    public static Task<TResult> MatchAsync<T, TResult>(this Option<T> option, Func<T, Task<TResult>> some, Func<Task<TResult>> none) =>
        Match(option, some, none);
}
