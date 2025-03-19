namespace Tyne;

/// <summary>
///     Extension methods for working with <see cref="Error"/>s.
/// </summary>
/// <remarks>
///     This contains multiple functional extensions for <see cref="Error"/>s.
/// </remarks>
public static class ErrorExtensions
{
    /// <summary>
    ///     Converts <paramref name="error"/> to an <c>Error</c> <see cref="Result{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of value the result encapsulates.</typeparam>
    /// <param name="error">The <see cref="Error"/>.</param>
    /// <returns>An <c>Error</c> <see cref="Result{T}"/>.</returns>
    public static Result<T> ToResult<T>(this Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        return new Result<T>(error);
    }

    /// <summary>
    ///     Creates a <see cref="ValueTask{TResult}"/> whose result is <paramref name="error"/>.
    /// </summary>
    /// <param name="error">The <see cref="Error"/>.</param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}"/> which, when <see langword="await"/>ed, returns <paramref name="error"/>.
    /// </returns>
    /// <remarks>
    ///     This is created synchronously.
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="error"/> is <see langword="null"/>.</exception>
    public static ValueTask<Error> ToValueTask(this Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        return ValueTask.FromResult(error);
    }

    /// <summary>
    ///     Creates a <see cref="Task{TResult}"/> whose result is <paramref name="error"/>.
    /// </summary>
    /// <param name="error">The <see cref="Error"/>.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> which, when <see langword="await"/>ed, returns <paramref name="error"/>.
    /// </returns>
    /// <remarks>
    ///     This is created synchronously.
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="error"/> is <see langword="null"/>.</exception>
    public static Task<Error> ToTask(this Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        return Task.FromResult(error);
    }
}
