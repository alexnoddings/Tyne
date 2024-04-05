using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Tyne;

/// <summary>
///     Static methods for creating <see cref="Result{T}"/>s.
/// </summary>
/// <seealso cref="Result{T}"/>
public static class Result
{
    /// <summary>
    ///     Creates an <c>Ok(<typeparamref name="T"/>)</c> <see cref="Result{T}"/> (i.e. <see cref="Result{T}.IsOk"/> is <see langword="true"/>).
    /// </summary>
    /// <typeparam name="T">The type of value the result encapsulates.</typeparam>
    /// <param name="value">The <typeparamref name="T"/> to wrap.</param>
    /// <returns>An <c>Ok(<typeparamref name="T"/>)</c> <see cref="Result{T}"/> which wraps <paramref name="value"/>.</returns>
    /// <remarks>
    ///     A <see cref="BadResultException"/> will be thrown if <paramref name="value"/> is <see langword="null"/>.
    /// </remarks>
    /// <exception cref="BadResultException">When <paramref name="value"/> is <see langword="null"/>.</exception>
    [Pure]
    // Method looks longer than AggressiveInlining would usually support,
    // but when inlined for a given T, the unnecessary branches can
    // be culled to result in a relatively small amount of asm.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T> Ok<T>(in T value)
    {
        if (value is null)
            throw new BadResultException(ExceptionMessages.Result_OkMustHaveValue);

        // Checking for value type first helps the JIT avoid running any caching checks for ref types
        if (typeof(T).IsValueType)
        {
            // Only the relevant branches are kept for value-type generic instantiations
            if (typeof(T) == typeof(Unit))
            {
                // Unit only has one possible value.
                // Unsafe.As avoids dynamic type checks from casting since we know T is Unit.
                return Unsafe.As<Result<T>>(Cache.OkUnit);
            }
            else if (typeof(T) == typeof(bool))
            {
                // Cache both true and false bools.
                // Can't Unsafe.As a generic T into a bool as only ref types are supported.
                var val = (bool)(object)value;
                var result = val ? Cache.OkTrue : Cache.OkFalse;
                return Unsafe.As<Result<T>>(result);
            }
            else if (typeof(T) == typeof(int))
            {
                // Only cache the int 0
                var val = (int)(object)value;
                if (val == 0)
                    return Unsafe.As<Result<T>>(Cache.OkIntZero);
            }
            else if (typeof(T) == typeof(Guid))
            {
                // Only cache the empty guid
                var val = (Guid)(object)value;
                if (val == Guid.Empty)
                    return Unsafe.As<Result<T>>(Cache.OkGuidEmpty);
            }
        }

        return new(value);
    }

    /// <summary>
    ///     Creates an <c>Error</c> <see cref="Result{T}"/> (i.e. <see cref="Result{T}.IsOk"/> is <see langword="false"/>).
    /// </summary>
    /// <typeparam name="T">The type of value the result encapsulates.</typeparam>
    /// <param name="message">The error message to construct the <see cref="Result{T}.Error"/> with.</param>
    /// <returns>An <c>Error</c> <see cref="Result{T}"/> whose error is constructed using <paramref name="message"/>.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T> Error<T>(string message)
    {
        // Let Error handle invalid codes/messages
        var error = new Error(Tyne.Error.DefaultCode, message, null);
        return new Result<T>(error);
    }

    /// <summary>
    ///     Creates an <c>Error</c> <see cref="Result{T}"/> (i.e. <see cref="Result{T}.IsOk"/> is <see langword="false"/>).
    /// </summary>
    /// <typeparam name="T">The type of value the result encapsulates.</typeparam>
    /// <param name="code">The error code to construct the <see cref="Result{T}.Error"/> with.</param>
    /// <param name="message">The error message to construct the <see cref="Result{T}.Error"/> with.</param>
    /// <returns>An <c>Error</c> <see cref="Result{T}"/> whose error is constructed using <paramref name="code"/> and <paramref name="message"/>.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T> Error<T>(string code, string message)
    {
        // Let Error handle invalid codes/messages
        var error = new Error(code, message, null);
        return new Result<T>(error);
    }

    /// <summary>
    ///     Creates an <c>Error</c> <see cref="Result{T}"/> (i.e. <see cref="Result{T}.IsOk"/> is <see langword="false"/>).
    /// </summary>
    /// <typeparam name="T">The type of value the result encapsulates.</typeparam>
    /// <param name="code">The error code to construct the <see cref="Result{T}.Error"/> with.</param>
    /// <param name="message">The error message to construct the <see cref="Result{T}.Error"/> with.</param>
    /// <param name="causedBy">The exception to construct the <see cref="Result{T}.Error"/> with.</param>
    /// <returns>An <c>Error</c> <see cref="Result{T}"/> whose error is constructed using <paramref name="code"/>, <paramref name="message"/>, and <paramref name="causedBy"/>.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T> Error<T>(string code, string message, Exception causedBy)
    {
        // Let Error handle invalid codes/messages
        var error = new Error(code, message, causedBy);
        return new Result<T>(error);
    }

    /// <summary>
    ///     Creates an <c>Error</c> <see cref="Result{T}"/> (i.e. <see cref="Result{T}.IsOk"/> is <see langword="false"/>).
    /// </summary>
    /// <typeparam name="T">The type of value the result encapsulates.</typeparam>
    /// <param name="error">The error to use as <see cref="Result{T}.Error"/>.</param>
    /// <returns>An <c>Error</c> <see cref="Result{T}"/> whose error is constructed using <paramref name="error"/>.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T> Error<T>(in Error error) =>
        new(error);

    /// <summary>
    ///     Caches common and simple result types.
    /// </summary>
    internal static class Cache
    {
        public static readonly Result<Unit> OkUnit = new(Unit.Value);
        public static readonly Result<bool> OkTrue = new(true);
        public static readonly Result<bool> OkFalse = new(false);
        public static readonly Result<int> OkIntZero = new(0);
        public static readonly Result<Guid> OkGuidEmpty = new(Guid.Empty);
    }
}
