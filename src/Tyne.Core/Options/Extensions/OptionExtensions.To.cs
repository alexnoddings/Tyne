using System.Runtime.CompilerServices;

namespace Tyne;

public static partial class OptionExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask<Option<T>> ToValueTask<T>(this Option<T> option) =>
        ValueTask.FromResult(option);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Option<T>> ToTask<T>(this Option<T> option) =>
        Task.FromResult(option);

    public static Result<T, TE> ToResult<T, TE>(this Option<T> option, TE error)
    {
        ArgumentNullException.ThrowIfNull(error);

        if (option.TryUnwrap(out var value))
            return Result.Ok<T, TE>(value);

        return Result.Error<T, TE>(error);
    }

    public static Result<T, TE> ToResult<T, TE>(this Option<T> option, Func<TE> errorFactory)
    {
        ArgumentNullException.ThrowIfNull(errorFactory);

        if (option.TryUnwrap(out var value))
            return Result.Ok<T, TE>(value);

        var error = errorFactory();
        if (error is null)
            throw new ArgumentException("Error factory returned a null value.", nameof(errorFactory));

        return Result.Error<T, TE>(error);
    }
}
