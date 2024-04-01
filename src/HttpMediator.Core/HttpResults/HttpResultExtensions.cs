using System.Net;

namespace Tyne.HttpMediator;

/// <summary>
///     Extension methods for working with <see cref="HttpResult{T}"/>s.
/// </summary>
/// <remarks>
///     These methods are <see cref="HttpResult{T}"/>-specific implementations of the extensions provided by <see cref="ResultExtensions"/>.
/// </remarks>
public static class HttpResultExtensions
{
    /// <summary>
    ///     If <paramref name="result"/> is <c>Ok(<typeparamref name="T"/>)</c>, then executes <paramref name="ok"/> with <see cref="Result{T}.Value"/>.
    ///     Otherwise, does nothing.
    /// </summary>
    /// <typeparam name="T">The type of <c>Ok(<typeparamref name="T"/>)</c> value the <paramref name="result"/> encapsulates.</typeparam>
    /// <param name="result">The <see cref="HttpResult{T}"/>.</param>
    /// <param name="ok">An action which is invoked with <paramref name="result"/>'s value when it is <c>Ok(<typeparamref name="T"/>)</c>.</param>
    /// <returns>
    ///     The <see langword="ref"/> <paramref name="result"/> for chaining.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This is used to execute an <see cref="Action{T}"/> on <paramref name="result"/>'s value without unwrapping it.
    ///         To also apply an action for <c>Error</c>, use <see cref="Apply{T}(HttpResult{T}, Action{T}, Action{Error})"/>.
    ///     </para>
    ///     <para>
    ///         To return a modified value of <c>Ok(<typeparamref name="T"/>)</c>, use <see cref="Select{T, TResult}(HttpResult{T}, Func{T, TResult})"/>.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="ok"/> is <see langword="null"/>.</exception>
    public static HttpResult<T> Apply<T>(this HttpResult<T> result, Action<T> ok)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ok);

        if (result.IsOk)
            ok(result.Value);

        return result;
    }

    /// <summary>
    ///     If <paramref name="result"/> is <c>Ok(<typeparamref name="T"/>)</c>, then executes <paramref name="ok"/> with <see cref="Result{T}.Value"/>.
    ///     Otherwise, executes <paramref name="err"/> with <see cref="Result{T}.Error"/>.
    /// </summary>
    /// <typeparam name="T">The type of <c>Ok(<typeparamref name="T"/>)</c> value the <paramref name="result"/> encapsulates.</typeparam>
    /// <param name="result">The <see cref="HttpResult{T}"/>.</param>
    /// <param name="ok">An action which is invoked with <paramref name="result"/>'s value when it is <c>Ok(<typeparamref name="T"/>)</c>.</param>
    /// <param name="err">An action which is invoked with <paramref name="result"/>'r error when it is <c>Error</c>.</param>
    /// <returns>
    ///     The <see langword="ref"/> <paramref name="result"/> for chaining.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This is used to execute an <see cref="Action{T}"/> on <paramref name="result"/>'s value or error without unwrapping them.
    ///         To only apply an action for <c>Ok(<typeparamref name="T"/>)</c>, use <see cref="Apply{T}(HttpResult{T}, Action{T})"/>.
    ///     </para>
    ///     <para>
    ///         To return a new value, use <see cref="ResultExtensions.Match{T, TResult}(Result{T}, Func{T, TResult}, Func{Error, TResult})"/>.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="ok"/> or <paramref name="err"/> are <see langword="null"/>.</exception>
    public static HttpResult<T> Apply<T>(this HttpResult<T> result, Action<T> ok, Action<Error> err)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ok);
        ArgumentNullException.ThrowIfNull(err);

        if (result.IsOk)
            ok(result.Value);
        else
            err(result.Error);

        return result;
    }

    /// <summary>
    ///     If <paramref name="result"/> is <c>Ok(<typeparamref name="T"/>)</c>, then applies <paramref name="selector"/> to it's value.
    ///     Otherwise, returns <c>Error</c>.
    /// </summary>
    /// <typeparam name="T">The type of <c>Ok(<typeparamref name="T"/>)</c> value the <paramref name="result"/> encapsulates.</typeparam>
    /// <typeparam name="TResult">The type to return.</typeparam>
    /// <param name="result">The <see cref="HttpResult{T}"/>.</param>
    /// <param name="selector">
    ///     A function which transforms <paramref name="result"/>'s <typeparamref name="T"/> value into a <typeparamref name="TResult"/>.
    ///     If this returns <see langword="null"/>, the returned <see cref="Error"/> will be <c>Error</c>. This will use a default <see cref="Error"/>.
    /// </param>
    /// <returns>
    ///     If <paramref name="selector"/> returns a non-<see langword="null"/> value, then this returns <c>Some(<typeparamref name="TResult"/>)</c>.
    ///     Otherwise, returns <c>Error</c>. This will use a default <see cref="Error"/>.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         If <paramref name="selector"/> returns <see langword="null"/>, then a default <see cref="Error"/> is used.
    ///         Instead, consider using <see cref="ResultExtensions.Select{T, TResult}(Result{T}, Func{T, TResult}, Func{Error})"/> to specify the error to use.
    ///     </para>
    ///     <para>
    ///         This may be used to safely transform <typeparamref name="T"/> into <typeparamref name="TResult"/> if <paramref name="result"/> is <c>Ok(<typeparamref name="T"/>)</c>.
    ///         Otherwise, <paramref name="selector"/> is ignored and <c>Error</c> is returned.
    ///     </para>
    ///     <para>
    ///         This may be chained with other <see cref="Select{T, TResult}(HttpResult{T}, Func{T, TResult})"/>s
    ///         and terminated with an <see cref="ResultExtensions.Or{T}(Result{T}, T)"/> to safely transform a result.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="selector"/> is <see langword="null"/>.</exception>
    public static HttpResult<TResult> Select<T, TResult>(this HttpResult<T> result, Func<T, TResult?> selector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(selector);

        if (!result.IsOk)
            return HttpResult.Error<TResult>(result.Error, result.StatusCode);

        var value = selector(result.Value);
        if (value is null)
            return HttpResult.Error<TResult>(Error.Default, HttpStatusCode.BadRequest);

        return HttpResult.Ok(value, result.StatusCode);
    }

    /// <summary>
    ///     If <paramref name="result"/> is <c>Ok(<typeparamref name="T"/>)</c>, then applies <paramref name="selector"/> to it's value.
    ///     Otherwise, returns <c>Error</c>.
    /// </summary>
    /// <typeparam name="T">The type of <c>Ok(<typeparamref name="T"/>)</c> value the <paramref name="result"/> encapsulates.</typeparam>
    /// <typeparam name="TResult">The type to return.</typeparam>
    /// <param name="result">The <see cref="HttpResult{T}"/>.</param>
    /// <param name="selector">
    ///     A function which transforms <paramref name="result"/>'s <typeparamref name="T"/> value into a <typeparamref name="TResult"/>.
    ///     If this returns <see langword="null"/>, the returned <see cref="Error"/> will be <c>Error</c>. This will use a default <see cref="Error"/>.
    /// </param>
    /// <param name="nullSelector">
    ///     A function which returns an <see cref="Error"/>.
    ///     This is executed if <paramref name="result"/> is <c>Ok(<typeparamref name="T"/>)</c> but <paramref name="selector"/> returns a <see langword="null"/> value.
    /// </param>
    /// <returns>
    ///     If <paramref name="selector"/> returns a non-<see langword="null"/> value, then this returns <c>Some(<typeparamref name="TResult"/>)</c>.
    ///     Otherwise, returns <c>Error</c>. If <paramref name="result"/> is <c>Error</c>, then the error is preserved. Otherwise, <paramref name="nullSelector"/> is invoked to return an error.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This may be used to safely transform <typeparamref name="T"/> into <typeparamref name="TResult"/> if <paramref name="result"/> is <c>Ok(<typeparamref name="T"/>)</c>.
    ///         Otherwise, <paramref name="selector"/> is ignored and <c>Error</c> is returned.
    ///     </para>
    ///     <para>
    ///         This may be chained with other <see cref="Select{T, TResult}(HttpResult{T}, Func{T, TResult})"/>s
    ///         and terminated with an <see cref="ResultExtensions.Or{T}(Result{T}, T)"/> to safely transform a result.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="selector"/> or <paramref name="nullSelector"/> are <see langword="null"/>.</exception>
    public static HttpResult<TResult> Select<T, TResult>(this HttpResult<T> result, Func<T, TResult?> selector, Func<(Error, HttpStatusCode)> nullSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(selector);
        ArgumentNullException.ThrowIfNull(nullSelector);

        if (!result.IsOk)
            return HttpResult.Error<TResult>(result.Error, result.StatusCode);

        var value = selector(result.Value);
        if (value is null)
        {
            var (error, statusCode) = nullSelector();
            return HttpResult.Error<TResult>(error, statusCode);
        }

        return HttpResult.Ok(value, result.StatusCode);
    }

    /// <summary>
    ///     Creates a <see cref="ValueTask{TResult}"/> whose result is <paramref name="result"/>.
    /// </summary>
    /// <typeparam name="T">The type of <c>Ok(<typeparamref name="T"/>)</c> value the <paramref name="result"/> encapsulates.</typeparam>
    /// <param name="result">The <see cref="HttpResult{T}"/>.</param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}"/> which, when <see langword="await"/>ed, returns <paramref name="result"/>.
    /// </returns>
    /// <remarks>
    ///     This is created synchronously.
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="result"/> is <see langword="null"/>.</exception>
    public static ValueTask<HttpResult<T>> ToValueTask<T>(this HttpResult<T> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return ValueTask.FromResult(result);
    }

    /// <summary>
    ///     Creates a <see cref="Task{TResult}"/> whose result is <paramref name="result"/>.
    /// </summary>
    /// <typeparam name="T">The type of <c>Ok(<typeparamref name="T"/>)</c> value the <paramref name="result"/> encapsulates.</typeparam>
    /// <param name="result">The <see cref="HttpResult{T}"/>.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> which, when <see langword="await"/>ed, returns <paramref name="result"/>.
    /// </returns>
    /// <remarks>
    ///     This is created synchronously.
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="result"/> is <see langword="null"/>.</exception>
    public static Task<HttpResult<T>> ToTask<T>(this HttpResult<T> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return Task.FromResult(result);
    }
}
