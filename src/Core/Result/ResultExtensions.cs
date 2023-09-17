using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Tyne;

/// <summary>
///     Extension methods for working with <see cref="Result{T}"/>s.
/// </summary>
/// <remarks>
///     Contains functional extensions for <see cref="Result{T}"/>s.
///     Most methods have overloads.
///     <list type="bullet">
///         <item>
///             <c>Apply</c> conditionally executes an <see cref="Action"/>.
///         </item>
///         <item>
///             <c>Match</c> returns a <c>T</c> based on if the result is <c>Ok(T)</c> or <c>Error</c>.
///         </item>
///         <item>
///             <c>Or</c> returns <see cref="Result{T}.Value"/> if it is <c>Ok(T)</c>, otherwise a fall-back value.
///             This is useful to safely unwrap results.
///         </item>
///         <item>
///             <c>Select</c> projects an <see cref="Result{T}.Value"/> into a new <see cref="Result{T}"/>.
///             This is only executed for <c>Ok(T)</c>s.
///         </item>
///         <item>
///             <c>ToValue/Task</c> wraps a <see cref="Result{T}"/> in a <see cref="Task"/>/<see cref="ValueTask"/>.
///         </item>
///         <item>
///             <c>Unwrap</c> returns <see cref="Result{T}.Value"/>, or throws an exception if it is <c>Error</c>.
///             This is better than accessing <see cref="Result{T}.Value"/> directly, though <c>Or</c> is still preferable to handle the bad path.
///         </item>
///     </list>
/// </remarks>
public static class ResultExtensions
{
    /// <summary>
    ///     If <paramref name="result"/> is <c>Ok(<typeparamref name="T"/>)</c>, then executes <paramref name="ok"/> with <see cref="Result{T}.Value"/>.
    ///     Otherwise, does nothing.
    /// </summary>
    /// <typeparam name="T">The type of <c>Ok(<typeparamref name="T"/>)</c> value the <paramref name="result"/> encapsulates.</typeparam>
    /// <param name="result">The <see langword="ref"/> <see cref="Error"/>.</param>
    /// <param name="ok">An action which is invoked with <paramref name="result"/>'s value when it is <c>Ok(<typeparamref name="T"/>)</c>.</param>
    /// <returns>
    ///     The <see langword="ref"/> <paramref name="result"/> for chaining.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This is used to execute an <see cref="Action{T}"/> on <paramref name="result"/>'s value without unwrapping it.
    ///         To also apply an action for <c>Error</c>, use <see cref="Apply{T}(Result{T}, Action{T}, Action{Error})"/>.
    ///     </para>
    ///     <para>
    ///         To return a modified value of <c>Ok(<typeparamref name="T"/>)</c>, use <see cref="Select{T, TResult}(Result{T}, Func{T, TResult})"/>.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="ok"/> is <see langword="null"/>.</exception>
    public static Result<T> Apply<T>(this Result<T> result, Action<T> ok)
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
    /// <param name="result">The <see langword="ref"/> <see cref="Error"/>.</param>
    /// <param name="ok">An action which is invoked with <paramref name="result"/>'s value when it is <c>Ok(<typeparamref name="T"/>)</c>.</param>
    /// <param name="err">An action which is invoked with <paramref name="result"/>'r error when it is <c>Error</c>.</param>
    /// <returns>
    ///     The <see langword="ref"/> <paramref name="result"/> for chaining.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This is used to execute an <see cref="Action{T}"/> on <paramref name="result"/>'s value or error without unwrapping them.
    ///         To only apply an action for <c>Ok(<typeparamref name="T"/>)</c>, use <see cref="Apply{T}(Result{T}, Action{T})"/>.
    ///     </para>
    ///     <para>
    ///         To return a new value, use <see cref="Match{T, TResult}(Result{T}, Func{T, TResult}, Func{Error, TResult})"/>.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="ok"/> or <paramref name="err"/> are <see langword="null"/>.</exception>
    public static Result<T> Apply<T>(this Result<T> result, Action<T> ok, Action<Error> err)
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
    ///     Pattern matches <paramref name="result"/>, returning a <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="T">The type of <c>Ok(<typeparamref name="T"/>)</c> value the <paramref name="result"/> encapsulates.</typeparam>
    /// <typeparam name="TResult">The type to return.</typeparam>
    /// <param name="result">The <see cref="Error"/>.</param>
    /// <param name="ok">
    ///     A function which is invoked with <paramref name="result"/>'s value when it is <c>Ok(<typeparamref name="T"/>)</c>, and returns a <typeparamref name="TResult"/>.
    /// </param>
    /// <param name="err">
    ///     A function which is invoked when <paramref name="result"/>'s error when it is <c>Error</c>, and returns a <typeparamref name="TResult"/>.
    /// </param>
    /// <returns>A <typeparamref name="TResult"/> returned by either <paramref name="ok"/> or <paramref name="err"/>.</returns>
    /// <remarks>
    ///     <para>
    ///         If <paramref name="result"/> is <c>Ok(<typeparamref name="T"/>)</c>, invokes and returns <paramref name="ok"/>.
    ///         Else if <paramref name="result"/> is <c>Error</c>, invokes and returns <paramref name="err"/>.
    ///     </para>
    ///     <para>
    ///         This is similar to C#'s <see langword="switch" /> pattern.
    ///     </para>
    ///     <para>
    ///         <see cref="Apply{T}(Result{T}, Action{T}, Action{Error})"/> may be used if a return value isn't needed.
    ///     </para>
    ///     <para>
    ///         <see cref="Select{T, TResult}(Result{T}, Func{T, TResult})"/> may be used to only handle the <c>Ok(<typeparamref name="T"/>)</c> case.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="ok"/> or <paramref name="err"/> are <see langword="null"/>.</exception>
    public static TResult Match<T, TResult>(this Result<T> result, Func<T, TResult> ok, Func<Error, TResult> err)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ok);
        ArgumentNullException.ThrowIfNull(err);

        return result.IsOk
            ? ok(result.Value)
            : err(result.Error);
    }

    /// <summary>
    ///     If <paramref name="result"/> is <c>Ok(<typeparamref name="T"/>)</c>, then returns <paramref name="result"/>'s value.
    ///     Otherwise, returns <paramref name="value"/>.
    /// </summary>
    /// <typeparam name="T">The type of <c>Ok(<typeparamref name="T"/>)</c> value the <paramref name="result"/> encapsulates.</typeparam>
    /// <param name="result">The <see cref="Error"/>.</param>
    /// <param name="value">
    ///     An alternative value to return when <paramref name="result"/> is <c>Error</c>.
    ///     This may not be <see langword="null"/>; use <see cref="OrDefault{T}(Result{T})"/> instead to return a <see langword="null"/> value.
    /// </param>
    /// <returns><paramref name="result"/>'s value or <paramref name="value"/>.</returns>
    /// <remarks>
    ///     <para>
    ///         This may be used to safely unwrap an <see cref="Error"/> by providing an alternative value.
    ///     </para>
    ///     <para>
    ///         This overload takes an instance of <typeparamref name="T"/>.
    ///         This may not be appropriate if <paramref name="value"/> requires computation, for example:
    ///         <code>
    ///             result.Or(new Entity
    ///             {
    ///                 // expensive initialisation
    ///             });
    ///         </code>
    ///         This will perform the expensive initialisation regardless of <paramref name="result"/>'s state.
    ///         Instead, consider using <see cref="Or{T}(Result{T}, Func{T})"/>.
    ///     </para>
    ///     <para>
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="value"/> is <see langword="null"/>.</exception>
    [Pure]
    [return: NotNullIfNotNull(nameof(value))]
    public static T Or<T>(this Result<T> result, T value)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(value);

        return result.IsOk
            ? result.Value!
            : value;
    }

    /// <summary>
    ///     If <paramref name="result"/> is <c>Ok(<typeparamref name="T"/>)</c>, then returns <paramref name="result"/>'s value.
    ///     Otherwise, invokes and returns <paramref name="valueFactory"/>.
    /// </summary>
    /// <typeparam name="T">The type of <c>Ok(<typeparamref name="T"/>)</c> value the <paramref name="result"/> encapsulates.</typeparam>
    /// <param name="result">The <see cref="Result{T}"/>.</param>
    /// <param name="valueFactory">
    ///     A function which returns an alternative value to use when <paramref name="result"/> is <c>Error</c>.
    ///     Although this may return <see langword="null"/>, you should use <see cref="OrDefault{T}(Result{T})"/> instead to return a <see langword="null"/> value.
    /// </param>
    /// <returns><paramref name="result"/>'s value or <paramref name="valueFactory"/>'s result when invoked.</returns>
    /// <remarks>
    ///     <para>
    ///         This may be used to safely unwrap an <see cref="Result{T}"/> by providing an alternative value.
    ///     </para>
    ///     <para>
    ///         This overload takes a factory which returns an instance of <typeparamref name="T"/>.
    ///         This is appropriate if <typeparamref name="T"/> requires computation, for example:
    ///         <code>
    ///             result.Or(() => new Entity
    ///             {
    ///                 // expensive initialisation
    ///             });
    ///         </code>
    ///         This expensive initialisation will ONLY be performed if <paramref name="result"/> is <c>Error</c>.
    ///         Otherwise, <paramref name="valueFactory"/> is not invoked, saving the computation cost.
    ///     </para>
    ///     <para>
    ///         However, this comes with a slight overhead of allocation/executing the <paramref name="valueFactory"/>.
    ///         This is better than performing expensive computation, but if your value is simple
    ///         (e.g. an <see cref="int"/>, <see cref="Guid"/>, or existing reference)
    ///         then you should consider using <see cref="Or{T}(Result{T}, T)"/> instead.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="valueFactory"/> is <see langword="null"/>.</exception>
    public static T Or<T>(this Result<T> result, Func<T> valueFactory)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(valueFactory);

        return result.IsOk
            ? result.Value
            : valueFactory();
    }

    /// <summary>
    ///     If <paramref name="result"/> is <c>Ok(<typeparamref name="T"/>)</c>, then returns <paramref name="result"/>'s value.
    ///     Otherwise, returns <c><see langword="default"/>(<typeparamref name="T"/>)</c>.
    /// </summary>
    /// <typeparam name="T">The type of <c>Ok(<typeparamref name="T"/>)</c> value the <paramref name="result"/> encapsulates.</typeparam>
    /// <param name="result">The <see cref="Result{T}"/>.</param>
    /// <returns><paramref name="result"/>'s value or <c><see langword="default"/>(<typeparamref name="T"/>)</c>.</returns>
    /// <remarks>
    ///     <para>
    ///         This may be used to safely unwrap an <see cref="Result{T}"/> by alternatively using <c><see langword="default"/>(<typeparamref name="T"/>)</c>.
    ///     </para>
    ///     <para>
    ///         <see cref="Or{T}(Result{T}, T)"/> may instead be used to provide a non-<c><see langword="default"/>(<typeparamref name="T"/>)</c> alternative value for <typeparamref name="T"/>.
    ///     </para>
    /// </remarks>
    [Pure]
    public static T? OrDefault<T>(this Result<T> result)
    {
        ArgumentNullException.ThrowIfNull(result);
        return result.IsOk
            ? result.Value
            : default;
    }

    /// <summary>
    ///     If <paramref name="result"/> is <c>Ok(<typeparamref name="T"/>)</c>, then applies <paramref name="selector"/> to it's value.
    ///     Otherwise, returns <c>Error</c>.
    /// </summary>
    /// <typeparam name="T">The type of <c>Ok(<typeparamref name="T"/>)</c> value the <paramref name="result"/> encapsulates.</typeparam>
    /// <typeparam name="TResult">The type to return.</typeparam>
    /// <param name="result">The <see cref="Error"/>.</param>
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
    ///         Instead, consider using <see cref="Select{T, TResult}(Result{T}, Func{T, TResult}, Func{Error})"/> to specify the error to use.
    ///     </para>
    ///     <para>
    ///         This may be used to safely transform <typeparamref name="T"/> into <typeparamref name="TResult"/> if <paramref name="result"/> is <c>Ok(<typeparamref name="T"/>)</c>.
    ///         Otherwise, <paramref name="selector"/> is ignored and <c>Error</c> is returned.
    ///     </para>
    ///     <para>
    ///         This may be chained with other <see cref="Select{T, TResult}(Result{T}, Func{T, TResult})"/>s
    ///         and terminated with an <see cref="Or{T}(Result{T}, T)"/> to safely transform a result.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="selector"/> is <see langword="null"/>.</exception>
    public static Result<TResult> Select<T, TResult>(this Result<T> result, Func<T, TResult?> selector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(selector);

        if (!result.IsOk)
            return Result.Error<TResult>(result.Error);

        var value = selector(result.Value);
        if (value is null)
            return Result.Error<TResult>(Error.Default);

        return Result.Ok(value);
    }

    /// <summary>
    ///     If <paramref name="result"/> is <c>Ok(<typeparamref name="T"/>)</c>, then applies <paramref name="selector"/> to it's value.
    ///     Otherwise, returns <c>Error</c>.
    /// </summary>
    /// <typeparam name="T">The type of <c>Ok(<typeparamref name="T"/>)</c> value the <paramref name="result"/> encapsulates.</typeparam>
    /// <typeparam name="TResult">The type to return.</typeparam>
    /// <param name="result">The <see cref="Error"/>.</param>
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
    ///         This may be chained with other <see cref="Select{T, TResult}(Result{T}, Func{T, TResult})"/>s
    ///         and terminated with an <see cref="Or{T}(Result{T}, T)"/> to safely transform a result.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="selector"/> or <paramref name="nullSelector"/> are <see langword="null"/>.</exception>
    public static Result<TResult> Select<T, TResult>(this Result<T> result, Func<T, TResult?> selector, Func<Error> nullSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(selector);
        ArgumentNullException.ThrowIfNull(nullSelector);

        if (!result.IsOk)
            return Result.Error<TResult>(result.Error);

        var value = selector(result.Value);
        if (value is null)
            return Result.Error<TResult>(nullSelector());

        return Result.Ok(value);
    }

    /// <summary>
    ///     Creates a <see cref="ValueTask{TResult}"/> whose result is <paramref name="result"/>.
    /// </summary>
    /// <typeparam name="T">The type of <c>Ok(<typeparamref name="T"/>)</c> value the <paramref name="result"/> encapsulates.</typeparam>
    /// <param name="result">The <see cref="Result{T}"/>.</param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}"/> which, when <see langword="await"/>ed, returns <paramref name="result"/>.
    /// </returns>
    /// <remarks>
    ///     This is created synchronously.
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="result"/> is <see langword="null"/>.</exception>
    public static ValueTask<Result<T>> ToValueTask<T>(this Result<T> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return ValueTask.FromResult(result);
    }

    /// <summary>
    ///     Creates a <see cref="Task{TResult}"/> whose result is <paramref name="result"/>.
    /// </summary>
    /// <typeparam name="T">The type of <c>Ok(<typeparamref name="T"/>)</c> value the <paramref name="result"/> encapsulates.</typeparam>
    /// <param name="result">The <see cref="Result{T}"/>.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> which, when <see langword="await"/>ed, returns <paramref name="result"/>.
    /// </returns>
    /// <remarks>
    ///     This is created synchronously.
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="result"/> is <see langword="null"/>.</exception>
    public static Task<Result<T>> ToTask<T>(this Result<T> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return Task.FromResult(result);
    }

    /// <summary>
    ///     Unwraps the <typeparamref name="T"/> which <paramref name="result"/> encapsulates.
    /// </summary>
    /// <typeparam name="T">The type of <c>Ok(<typeparamref name="T"/>)</c> value the <paramref name="result"/> encapsulates.</typeparam>
    /// <param name="result">The <see cref="Error"/>.</param>
    /// <returns>
    ///     The <typeparamref name="T"/> which <paramref name="result"/> encapsulates.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This assumes <paramref name="result"/> is <c>Ok(<typeparamref name="T"/>)</c>.
    ///         If it is <c>Error</c>, this will throw an <see cref="UnwrapResultException"/>.
    ///     </para>
    ///     <para>
    ///         Consider using <see cref="Unwrap{T}(Result{T}, string)"/> to provide
    ///         a custom exception message if <paramref name="result"/> is <c>Error</c>.
    ///     </para>
    /// </remarks>
    /// <exception cref="UnwrapResultException">When <paramref name="result"/> is <c>Error</c>.</exception>
    [Pure]
    public static T Unwrap<T>(this Result<T> result)
    {
        ArgumentNullException.ThrowIfNull(result);
        if (result.IsOk)
            return result.Value;

        throw new UnwrapResultException();
    }

    /// <summary>
    ///     Unwraps the <typeparamref name="T"/> which <paramref name="result"/> encapsulates.
    /// </summary>
    /// <typeparam name="T">The type of <c>Ok(<typeparamref name="T"/>)</c> value the <paramref name="result"/> encapsulates.</typeparam>
    /// <param name="result">The <see cref="Error"/>.</param>
    /// <param name="noneExceptionMessage">An error message to use if a <see cref="UnwrapResultException"/> is thrown.</param>
    /// <returns>
    ///     The <typeparamref name="T"/> which <paramref name="result"/> encapsulates.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This assumes <paramref name="result"/> is <c>Ok(<typeparamref name="T"/>)</c>.
    ///         If it is <c>Error</c>, this will throw an <see cref="UnwrapResultException"/> whose message is <paramref name="noneExceptionMessage"/>.
    ///         If this message is null or empty, a default message will be used instead.
    ///     </para>
    ///     <para>
    ///         This should be used if your <paramref name="noneExceptionMessage"/> is cheap, such as a constant or static string.
    ///         If it is expensive (e.g. an interpolated string), you should use <see cref="Unwrap{T}(Result{T}, Func{Exception})"/> instead.
    ///     </para>
    /// </remarks>
    /// <exception cref="UnwrapResultException">When <paramref name="result"/> is <c>Error</c>.</exception>
    [Pure]
    public static T Unwrap<T>(this Result<T> result, string noneExceptionMessage)
    {
        ArgumentNullException.ThrowIfNull(result);
        if (result.IsOk)
            return result.Value;

        throw new UnwrapResultException(noneExceptionMessage);
    }

    /// <summary>
    ///     Unwraps the <typeparamref name="T"/> which <paramref name="result"/> encapsulates.
    /// </summary>
    /// <typeparam name="T">The type of <c>Ok(<typeparamref name="T"/>)</c> value the <paramref name="result"/> encapsulates.</typeparam>
    /// <param name="result">The <see cref="Error"/>.</param>
    /// <param name="noneExceptionMessageFactory">
    ///     A function which returns an error message to use if a <see cref="UnwrapResultException"/> is thrown.
    ///     This is only invoked if <paramref name="result"/> can't be unwrapped.
    /// </param>
    /// <returns>
    ///     The <typeparamref name="T"/> which <paramref name="result"/> encapsulates.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This assumes <paramref name="result"/> is <c>Ok(<typeparamref name="T"/>)</c>.
    ///         If it is <c>Error</c>, this will throw an <see cref="UnwrapResultException"/>
    ///         whose message is the value returned from <paramref name="noneExceptionMessageFactory"/>.
    ///         If this message is null or empty, a default message will be used instead.
    ///     </para>
    ///     <para>
    ///         This should be used if your exception message is expensive, such as an interpolated string.
    ///         If it is cheap (e.g. a constant or static string), you should use <see cref="Unwrap{T}(Result{T}, string)"/> instead.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="noneExceptionMessageFactory"/> is <see langword="null"/>.</exception>
    /// <exception cref="UnwrapResultException">When <paramref name="result"/> is <c>Error</c>.</exception>
    public static T Unwrap<T>(this Result<T> result, Func<string> noneExceptionMessageFactory)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(noneExceptionMessageFactory);

        if (result.IsOk)
            return result.Value;

        var noneExceptionMessage = noneExceptionMessageFactory();
        throw new UnwrapResultException(noneExceptionMessage);
    }

    /// <summary>
    ///     Unwraps the <typeparamref name="T"/> which <paramref name="result"/> encapsulates.
    /// </summary>
    /// <typeparam name="T">The type of <c>Ok(<typeparamref name="T"/>)</c> value the <paramref name="result"/> encapsulates.</typeparam>
    /// <param name="result">The <see cref="Error"/>.</param>
    /// <param name="noneExceptionFactory">
    ///     A function which returns an <see cref="Exception"/> which is thrown if <paramref name="result"/> cannot be unwrapped.
    ///     This is only invoked if <paramref name="result"/> can't be unwrapped.
    /// </param>
    /// <returns>
    ///     The <typeparamref name="T"/> which <paramref name="result"/> encapsulates.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This assumes <paramref name="result"/> is <c>Ok(<typeparamref name="T"/>)</c>.
    ///         If it is <c>Error</c>, this will throw the exception returned by <paramref name="noneExceptionFactory"/>.
    ///         If this returns a <see langword="null"/> exception, a <see cref="ArgumentException"/> will be thrown.
    ///     </para>
    ///     <para>
    ///         Consider using <see cref="Unwrap{T}(Result{T}, string)"/> to provide
    ///         a custom exception message if <paramref name="result"/> is <c>Error</c>.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="noneExceptionFactory"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="noneExceptionFactory"/> returned a <see langword="null"/> exception.</exception>
    /// <exception cref="Exception">The exception returned by <paramref name="noneExceptionFactory"/> is thrown when <paramref name="result"/> is <c>Error</c>.</exception>
    public static T Unwrap<T>(this Result<T> result, Func<Exception> noneExceptionFactory)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(noneExceptionFactory);

        if (result.IsOk)
            return result.Value;

        var exception = noneExceptionFactory();
        throw exception ?? new ArgumentException("", nameof(noneExceptionFactory));
    }
}
