namespace Tyne;

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
}
