namespace Tyne;

public static class ResultExtensions
{
    public static Task<Result<T>> AsTask<T>(this Result<T> result)
    {
        ArgumentNullException.ThrowIfNull(result);
        return Task.FromResult(result);
    }
}
