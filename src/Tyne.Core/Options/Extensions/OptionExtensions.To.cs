namespace Tyne;

public static partial class OptionExtensions
{
    public static ValueTask<Option<T>> ToValueTask<T>(this Option<T> option) =>
        ValueTask.FromResult(option);

    public static Task<Option<T>> ToTask<T>(this Option<T> option) =>
        Task.FromResult(option);

    public static Result<T, E> ToResult<T, E>(this Option<T> option, E error)
    {
        ArgumentNullException.ThrowIfNull(error);

        if (option.TryUnwrap(out var value))
            return Result.Ok<T, E>(value);

        return Result.Error<T, E>(error);
    }

    public static Result<T, E> ToResult<T, E>(this Option<T> option, Func<E> errorFactory)
    {
        ArgumentNullException.ThrowIfNull(errorFactory);

        if (option.TryUnwrap(out var value))
            return Result.Ok<T, E>(value);

        var error = errorFactory();
        if (error is null)
            throw new ArgumentException("Error factory returned a null value.", nameof(errorFactory));

        return Result.Error<T, E>(error);
    }
}
