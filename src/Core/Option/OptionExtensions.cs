using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Tyne;

/// <summary>
///     Extension methods for working with <see cref="Option{T}"/>s.
/// </summary>
/// <remarks>
///     Contains functional extensions for <see cref="Option{T}"/>s.
///     Most methods have overloads.
///     <list type="bullet">
///         <item>
///             <c>Apply</c> conditionally executes an <see cref="Action"/>.
///         </item>
///         <item>
///             <c>AsValue/Task</c> wraps an <see cref="Option{T}"/> in a <see cref="Task"/>/<see cref="ValueTask"/>.
///         </item>
///         <item>
///             <c>Match</c> returns a <c>T</c> based on if the option is <c>Some(T)</c> or <c>None</c>.
///         </item>
///         <item>
///             <c>Or</c> returns <see cref="Option{T}.Value"/> if it is <c>Some(T)</c>, otherwise a fall-back value.
///             This is useful to safely unwrap options.
///         </item>
///         <item>
///             <c>Select</c> projects an <see cref="Option{T}.Value"/> into a new <see cref="Option{T}"/>.
///             This is only executed for <c>Some(T)</c>s.
///         </item>
///         <item>
///             <c>Unwrap</c> returns <see cref="Option{T}.Value"/>, or throws an exception if it is <c>None</c>.
///             This is better than accessing <see cref="Option{T}.Value"/> directly, though <c>Or</c> is still preferable to handle the bad path.
///         </item>
///     </list>
/// </remarks>
public static class OptionExtensions
{
    /// <summary>
    ///     If <paramref name="option"/> is <c>Some(<typeparamref name="T"/>)</c>, then executes <paramref name="some"/> with <see cref="Option{T}.Value"/>.
    ///     Otherwise, does nothing.
    /// </summary>
    /// <typeparam name="T">The type of <c>Some(<typeparamref name="T"/>)</c> value the <paramref name="option"/> encapsulates.</typeparam>
    /// <param name="option">The <see langword="ref"/> <see cref="Option{T}"/>.</param>
    /// <param name="some">An action which is invoked with <paramref name="option"/>'s value when it is <c>Some(<typeparamref name="T"/>)</c>.</param>
    /// <returns>
    ///     The <see langword="ref"/> <paramref name="option"/> for chaining.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This is used to execute an <see cref="Action{T}"/> on <paramref name="option"/>'s value without unwrapping it.
    ///         To also apply an action for <c>None</c>, use <see cref="Apply{T}(ref Option{T}, Action{T}, Action)"/>.
    ///     </para>
    ///     <para>
    ///         To return a modified value of <c>Some(<typeparamref name="T"/>)</c>, use <see cref="Select{T, TResult}(in Option{T}, Func{T, TResult})"/>.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="some"/> is <see langword="null"/>.</exception>
    public static ref Option<T> Apply<T>(this ref Option<T> option, Action<T> some)
    {
        ArgumentNullException.ThrowIfNull(some);

        if (option.HasValue)
            some(option.Value);

        return ref option;
    }

    /// <summary>
    ///     If <paramref name="option"/> is <c>Some(<typeparamref name="T"/>)</c>, then executes <paramref name="some"/> with <see cref="Option{T}.Value"/>.
    ///     Otherwise, executes <paramref name="none"/>.
    /// </summary>
    /// <typeparam name="T">The type of <c>Some(<typeparamref name="T"/>)</c> value the <paramref name="option"/> encapsulates.</typeparam>
    /// <param name="option">The <see langword="ref"/> <see cref="Option{T}"/>.</param>
    /// <param name="some">An action which is invoked with <paramref name="option"/>'s value when it is <c>Some(<typeparamref name="T"/>)</c>.</param>
    /// <param name="none">An action which is invoked when <paramref name="option"/> is <c>None</c>.</param>
    /// <returns>
    ///     The <see langword="ref"/> <paramref name="option"/> for chaining.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         If <paramref name="option"/> is <c>Some(<typeparamref name="T"/>)</c>, invokes <paramref name="some"/>.
    ///         Otherwise, invokes <paramref name="none"/>.
    ///     </para>
    ///     <para>
    ///         To return a new value, use <see cref="Match{T, TResult}(in Option{T}, Func{T, TResult}, Func{TResult})"/>.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="some"/> or <paramref name="none"/> are <see langword="null"/>.</exception>
    public static ref Option<T> Apply<T>(this ref Option<T> option, Action<T> some, Action none)
    {
        ArgumentNullException.ThrowIfNull(some);
        ArgumentNullException.ThrowIfNull(none);

        if (option.HasValue)
            some(option.Value);
        else
            none();

        return ref option;
    }

    /// <summary>
    ///     Creates a <see cref="ValueTask{TResult}"/> whose result is <paramref name="option"/>.
    /// </summary>
    /// <typeparam name="T">The type of <c>Some(<typeparamref name="T"/>)</c> value the <paramref name="option"/> encapsulates.</typeparam>
    /// <param name="option">The <see langword="ref"/> <see cref="Option{T}"/>.</param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}"/> which, when <see langword="await"/>ed, returns <paramref name="option"/>.
    /// </returns>
    /// <remarks>
    ///     This is created synchronously.
    /// </remarks>
    public static ValueTask<Option<T>> AsValueTask<T>(this ref Option<T> option) =>
        ValueTask.FromResult(option);

    /// <summary>
    ///     Creates a <see cref="Task{TResult}"/> whose result is <paramref name="option"/>.
    /// </summary>
    /// <typeparam name="T">The type of <c>Some(<typeparamref name="T"/>)</c> value the <paramref name="option"/> encapsulates.</typeparam>
    /// <param name="option">The <see langword="ref"/> <see cref="Option{T}"/>.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> which, when <see langword="await"/>ed, returns <paramref name="option"/>.
    /// </returns>
    /// <remarks>
    ///     This is created synchronously.
    /// </remarks>
    public static Task<Option<T>> AsTask<T>(this ref Option<T> option) =>
        Task.FromResult(option);

    /// <summary>
    ///     Pattern matches <paramref name="option"/>, returning a <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="T">The type of <c>Some(<typeparamref name="T"/>)</c> value the <paramref name="option"/> encapsulates.</typeparam>
    /// <typeparam name="TResult">The type to return.</typeparam>
    /// <param name="option">The <see cref="Option{T}"/>.</param>
    /// <param name="some">
    ///     A function which is invoked with <paramref name="option"/>'s value when it is <c>Some(<typeparamref name="T"/>)</c>, and returns a <typeparamref name="TResult"/>.
    /// </param>
    /// <param name="none">
    ///     A function which is invoked when <paramref name="option"/> is <c>None</c>, and returns a <typeparamref name="TResult"/>.
    /// </param>
    /// <returns>A <typeparamref name="TResult"/> returned by either <paramref name="some"/> or <paramref name="none"/>.</returns>
    /// <remarks>
    ///     <para>
    ///         If <paramref name="option"/> is <c>Some(<typeparamref name="T"/>)</c>, invokes and returns <paramref name="some"/>.
    ///         Else if <paramref name="option"/> is <c>None</c>, invokes and returns <paramref name="none"/>.
    ///     </para>
    ///     <para>
    ///         This is similar to C#'s <see langword="switch" /> pattern.
    ///     </para>
    ///     <para>
    ///         <see cref="Apply{T}(ref Option{T}, Action{T}, Action)"/> may be used if a return value isn't needed.
    ///     </para>
    ///     <para>
    ///         <see cref="Select{T, TResult}(in Option{T}, Func{T, TResult})"/> may be used to only handle the <c>Some(<typeparamref name="T"/>)</c> case.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="some"/> or <paramref name="none"/> are <see langword="null"/>.</exception>
    public static TResult Match<T, TResult>(this in Option<T> option, Func<T, TResult> some, Func<TResult> none)
    {
        ArgumentNullException.ThrowIfNull(some);
        ArgumentNullException.ThrowIfNull(none);

        return option.HasValue
            ? some(option.Value)
            : none();
    }

    /// <summary>
    ///     If <paramref name="option"/> is <c>Some(<typeparamref name="T"/>)</c>, then returns <paramref name="option"/>'s value.
    ///     Otherwise, returns <paramref name="value"/>.
    /// </summary>
    /// <typeparam name="T">The type of <c>Some(<typeparamref name="T"/>)</c> value the <paramref name="option"/> encapsulates.</typeparam>
    /// <param name="option">The <see cref="Option{T}"/>.</param>
    /// <param name="value">
    ///     An alternative value to return when <paramref name="option"/> is <c>None</c>.
    ///     This may not be <see langword="null"/>; use <see cref="OrDefault{T}(in Option{T})"/> instead to return a <see langword="null"/> value.
    /// </param>
    /// <returns><paramref name="option"/>'s value or <paramref name="value"/>.</returns>
    /// <remarks>
    ///     <para>
    ///         This may be used to safely unwrap an <see cref="Option{T}"/> by providing an alternative value.
    ///     </para>
    ///     <para>
    ///         This overload takes an instance of <typeparamref name="T"/>.
    ///         This may not be appropriate if <paramref name="value"/> requires computation, for example:
    ///         <code>
    ///             option.Or(new Entity
    ///             {
    ///                 // expensive initialisation
    ///             });
    ///         </code>
    ///         This will perform the expensive initialisation regardless of <paramref name="option"/>'s state.
    ///         Instead, consider using <see cref="Or{T}(in Option{T}, Func{T})"/>.
    ///     </para>
    ///     <para>
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="value"/> is <see langword="null"/>.</exception>
    [Pure]
    [return: NotNullIfNotNull(nameof(value))]
    public static T Or<T>(this in Option<T> option, T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return option.HasValue
            ? option.Value!
            : value;
    }

    /// <summary>
    ///     If <paramref name="option"/> is <c>Some(<typeparamref name="T"/>)</c>, then returns <paramref name="option"/>'s value.
    ///     Otherwise, invokes and returns <paramref name="valueFactory"/>.
    /// </summary>
    /// <typeparam name="T">The type of <c>Some(<typeparamref name="T"/>)</c> value the <paramref name="option"/> encapsulates.</typeparam>
    /// <param name="option">The <see cref="Option{T}"/>.</param>
    /// <param name="valueFactory">
    ///     A function which returns an alternative value to use when <paramref name="option"/> is <c>None</c>.
    ///     Although this may return <see langword="null"/>, you should use <see cref="OrDefault{T}(in Option{T})"/> instead to return a <see langword="null"/> value.
    /// </param>
    /// <returns><paramref name="option"/>'s value or <paramref name="valueFactory"/>'s result when invoked.</returns>
    /// <remarks>
    ///     <para>
    ///         This may be used to safely unwrap an <see cref="Option{T}"/> by providing an alternative value.
    ///     </para>
    ///     <para>
    ///         This overload takes a factory which returns an instance of <typeparamref name="T"/>.
    ///         This is appropriate if <typeparamref name="T"/> requires computation, for example:
    ///         <code>
    ///             option.Or(() => new Entity
    ///             {
    ///                 // expensive initialisation
    ///             });
    ///         </code>
    ///         This expensive initialisation will ONLY be performed if <paramref name="option"/> is <c>None</c>.
    ///         Otherwise, <paramref name="valueFactory"/> is not invoked, saving the computation cost.
    ///     </para>
    ///     <para>
    ///         However, this comes with a slight overhead of allocation/executing the <paramref name="valueFactory"/>.
    ///         This is better than performing expensive computation, but if your value is simple
    ///         (e.g. an <see cref="int"/>, <see cref="Guid"/>, or existing reference)
    ///         then you should consider using <see cref="Or{T}(in Option{T}, T)"/> instead.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="valueFactory"/> is <see langword="null"/>.</exception>
    public static T Or<T>(this in Option<T> option, Func<T> valueFactory)
    {
        ArgumentNullException.ThrowIfNull(valueFactory);

        return option.HasValue
            ? option.Value
            : valueFactory();
    }

    /// <summary>
    ///     If <paramref name="option"/> is <c>Some(<typeparamref name="T"/>)</c>, then returns <paramref name="option"/>'s value.
    ///     Otherwise, returns <c><see langword="default"/>(<typeparamref name="T"/>)</c>.
    /// </summary>
    /// <typeparam name="T">The type of <c>Some(<typeparamref name="T"/>)</c> value the <paramref name="option"/> encapsulates.</typeparam>
    /// <param name="option">The <see cref="Option{T}"/>.</param>
    /// <returns><paramref name="option"/>'s value or <c><see langword="default"/>(<typeparamref name="T"/>)</c>.</returns>
    /// <remarks>
    ///     <para>
    ///         This may be used to safely unwrap an <see cref="Option{T}"/> by alternatively using <c><see langword="default"/>(<typeparamref name="T"/>)</c>.
    ///     </para>
    ///     <para>
    ///         <see cref="Or{T}(in Option{T}, T)"/> may instead be used to provide a non-<c><see langword="default"/>(<typeparamref name="T"/>)</c> alternative value for <typeparamref name="T"/>.
    ///     </para>
    /// </remarks>
    [Pure]
    public static T? OrDefault<T>(this in Option<T> option)
    {
        return option.HasValue
            ? option.Value
            : default;
    }

    /// <summary>
    ///     If <paramref name="option"/> is <c>Some(<typeparamref name="T"/>)</c>, then returns <paramref name="option"/>'s value.
    ///     Otherwise, returns <c><see langword="null"/></c>.
    /// </summary>
    /// <typeparam name="T">The type of <c>Some(<typeparamref name="T"/>)</c> value the <paramref name="option"/> encapsulates.</typeparam>
    /// <param name="option">The <see cref="Option{T}"/>.</param>
    /// <returns><paramref name="option"/>'s value or <c><see langword="null"/></c>.</returns>
    /// <remarks>
    ///     <para>
    ///         This may be used to safely unwrap an <see cref="Option{T}"/> by alternatively using <c><see langword="null"/></c>.
    ///     </para>
    ///     <para>
    ///         <see cref="Or{T}(in Option{T}, T)"/> may instead be used to provide a non-<c><see langword="null"/></c> alternative value for <typeparamref name="T"/>.
    ///     </para>
    ///     <para>
    ///         This behaves identically to <see cref="OrDefault{T}(in Option{T})"/>, but only accepts reference types for <typeparamref name="T"/>.
    ///         This provides a more obvious method name in some use cases.
    ///     </para>
    /// </remarks>
    [Pure]
    public static T? OrNull<T>(this in Option<T> option) where T : class
    {
        return option.HasValue
            ? option.Value
            : null;
    }

    /// <summary>
    ///     If <paramref name="option"/> is <c>Some(<typeparamref name="T"/>)</c>, then applies <paramref name="selector"/> to it's value.
    ///     Otherwise, returns <c>None</c>.
    /// </summary>
    /// <typeparam name="T">The type of <c>Some(<typeparamref name="T"/>)</c> value the <paramref name="option"/> encapsulates.</typeparam>
    /// <typeparam name="TResult">The type to return.</typeparam>
    /// <param name="option">The <see cref="Option{T}"/>.</param>
    /// <param name="selector">
    ///     A function which transforms <paramref name="option"/>'s <typeparamref name="T"/> value into a <typeparamref name="TResult"/>.
    ///     If this returns <see langword="null"/>, the returned <see cref="Option{T}"/> will be <c>None</c>.
    /// </param>
    /// <returns>
    ///     If <paramref name="selector"/> returns a non-<see langword="null"/> value, then this returns <c>Some(<typeparamref name="TResult"/>)</c>.
    ///     Otherwise, returns <c>None</c>.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This may be used to safely transform <typeparamref name="T"/> into <typeparamref name="TResult"/> if <paramref name="option"/> is <c>Some(<typeparamref name="T"/>)</c>.
    ///         Otherwise, <paramref name="selector"/> is ignored and <c>None</c> is returned.
    ///     </para>
    ///     <para>
    ///         This may be chained with other <see cref="Select{T, TResult}(in Option{T}, Func{T, TResult})"/>s
    ///         and terminated with an <see cref="Or{T}(in Option{T}, T)"/> to safely transform an option.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="selector"/> is <see langword="null"/>.</exception>
    public static Option<TResult> Select<T, TResult>(this in Option<T> option, Func<T, TResult?> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        return option.HasValue
            ? Option.From(selector(option.Value))
            : Option.None<TResult>();
    }

    /// <summary>
    ///     Unwraps the <typeparamref name="T"/> which <paramref name="option"/> encapsulates.
    /// </summary>
    /// <typeparam name="T">The type of <c>Some(<typeparamref name="T"/>)</c> value the <paramref name="option"/> encapsulates.</typeparam>
    /// <param name="option">The <see cref="Option{T}"/>.</param>
    /// <returns>
    ///     The <typeparamref name="T"/> which <paramref name="option"/> encapsulates.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This assumes <paramref name="option"/> is <c>Some(<typeparamref name="T"/>)</c>.
    ///         If it is <c>None</c>, this will throw an <see cref="UnwrapOptionException"/>.
    ///     </para>
    ///     <para>
    ///         Consider using <see cref="Unwrap{T}(in Option{T}, string)"/> to provide
    ///         a custom exception message if <paramref name="option"/> is <c>None</c>.
    ///     </para>
    /// </remarks>
    /// <exception cref="UnwrapOptionException">When <paramref name="option"/> is <c>None</c>.</exception>
    [Pure]
    public static T Unwrap<T>(this in Option<T> option)
    {
        if (option.HasValue)
            return option.Value;

        throw new UnwrapOptionException();
    }

    /// <summary>
    ///     Unwraps the <typeparamref name="T"/> which <paramref name="option"/> encapsulates.
    /// </summary>
    /// <typeparam name="T">The type of <c>Some(<typeparamref name="T"/>)</c> value the <paramref name="option"/> encapsulates.</typeparam>
    /// <param name="option">The <see cref="Option{T}"/>.</param>
    /// <param name="noneExceptionMessage">An error message to use if a <see cref="UnwrapOptionException"/> is thrown.</param>
    /// <returns>
    ///     The <typeparamref name="T"/> which <paramref name="option"/> encapsulates.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This assumes <paramref name="option"/> is <c>Some(<typeparamref name="T"/>)</c>.
    ///         If it is <c>None</c>, this will throw an <see cref="UnwrapOptionException"/> whose message is <paramref name="noneExceptionMessage"/>.
    ///         If this message is null or empty, a default message will be used instead.
    ///     </para>
    ///     <para>
    ///         This should be used if your <paramref name="noneExceptionMessage"/> is cheap, such as a constant or static string.
    ///         If it is expensive (e.g. an interpolated string), you should use <see cref="Unwrap{T}(in Option{T}, Func{Exception})"/> instead.
    ///     </para>
    /// </remarks>
    /// <exception cref="UnwrapOptionException">When <paramref name="option"/> is <c>None</c>.</exception>
    [Pure]
    public static T Unwrap<T>(this in Option<T> option, string noneExceptionMessage)
    {
        if (option.HasValue)
            return option.Value;

        throw new UnwrapOptionException(noneExceptionMessage);
    }

    /// <summary>
    ///     Unwraps the <typeparamref name="T"/> which <paramref name="option"/> encapsulates.
    /// </summary>
    /// <typeparam name="T">The type of <c>Some(<typeparamref name="T"/>)</c> value the <paramref name="option"/> encapsulates.</typeparam>
    /// <param name="option">The <see cref="Option{T}"/>.</param>
    /// <param name="noneExceptionMessageFactory">
    ///     A function which returns an error message to use if a <see cref="UnwrapOptionException"/> is thrown.
    ///     This is only invoked if <paramref name="option"/> can't be unwrapped.
    /// </param>
    /// <returns>
    ///     The <typeparamref name="T"/> which <paramref name="option"/> encapsulates.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This assumes <paramref name="option"/> is <c>Some(<typeparamref name="T"/>)</c>.
    ///         If it is <c>None</c>, this will throw an <see cref="UnwrapOptionException"/>
    ///         whose message is the value returned from <paramref name="noneExceptionMessageFactory"/>.
    ///         If this message is null or empty, a default message will be used instead.
    ///     </para>
    ///     <para>
    ///         This should be used if your exception message is expensive, such as an interpolated string.
    ///         If it is cheap (e.g. a constant or static string), you should use <see cref="Unwrap{T}(in Option{T}, string)"/> instead.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="noneExceptionMessageFactory"/> is <see langword="null"/>.</exception>
    /// <exception cref="UnwrapOptionException">When <paramref name="option"/> is <c>None</c>.</exception>
    public static T Unwrap<T>(this in Option<T> option, Func<string> noneExceptionMessageFactory)
    {
        ArgumentNullException.ThrowIfNull(noneExceptionMessageFactory);

        if (option.HasValue)
            return option.Value;

        var noneExceptionMessage = noneExceptionMessageFactory();
        throw new UnwrapOptionException(noneExceptionMessage);
    }

    /// <summary>
    ///     Unwraps the <typeparamref name="T"/> which <paramref name="option"/> encapsulates.
    /// </summary>
    /// <typeparam name="T">The type of <c>Some(<typeparamref name="T"/>)</c> value the <paramref name="option"/> encapsulates.</typeparam>
    /// <param name="option">The <see cref="Option{T}"/>.</param>
    /// <param name="noneExceptionFactory">
    ///     A function which returns an <see cref="Exception"/> which is thrown if <paramref name="option"/> cannot be unwrapped.
    ///     This is only invoked if <paramref name="option"/> can't be unwrapped.
    /// </param>
    /// <returns>
    ///     The <typeparamref name="T"/> which <paramref name="option"/> encapsulates.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This assumes <paramref name="option"/> is <c>Some(<typeparamref name="T"/>)</c>.
    ///         If it is <c>None</c>, this will throw the exception returned by <paramref name="noneExceptionFactory"/>.
    ///         If this returns a <see langword="null"/> exception, a <see cref="ArgumentException"/> will be thrown.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="noneExceptionFactory"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">When <paramref name="noneExceptionFactory"/> returns a <see langword="null"/> exception.</exception>
    /// <exception cref="Exception">When <paramref name="option"/> is <c>None</c>, the exception returned by <paramref name="noneExceptionFactory"/> is thrown.</exception>
    public static T Unwrap<T>(this in Option<T> option, Func<Exception> noneExceptionFactory)
    {
        ArgumentNullException.ThrowIfNull(noneExceptionFactory);

        if (option.HasValue)
            return option.Value;

        var exception = noneExceptionFactory();
        throw exception ?? new ArgumentException("", nameof(noneExceptionFactory));
    }
}
