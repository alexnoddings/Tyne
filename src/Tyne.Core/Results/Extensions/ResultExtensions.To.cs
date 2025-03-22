using System.Runtime.CompilerServices;

namespace Tyne;

public static partial class ResultExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask<Result<T, TE>> ToValueTask<T, TE>(this Result<T, TE> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return ValueTask.FromResult(result);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Result<T, TE>> ToTask<T, TE>(this Result<T, TE> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return Task.FromResult(result);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> ToOption<T, TE>(this Result<T, TE> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (result.TryUnwrap(out var value, out _))
            return Option.Some(value);

        return Option.None<T>();
    }
}
