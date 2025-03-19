using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Tyne;

/// <summary>
///     Static methods for creating <see cref="Result{T, E}"/>s.
/// </summary>
/// <seealso cref="Result{T, E}"/>
public static partial class Result
{
    /// <summary>
    ///     Creates an <c>Ok(<typeparamref name="T"/>)</c> <see cref="Result{T, E}"/>.
    /// </summary>
    /// <typeparam name="T">The type of value the result encapsulates.</typeparam>
    /// <typeparam name="E">The type of error the result encapsulates.</typeparam>
    /// <param name="value">The <typeparamref name="T"/> value to encapsulate.</param>
    /// <returns>An <c>Ok(<typeparamref name="T"/>)</c> <see cref="Result{T, E}"/> which wraps <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="value"/> is <see langword="null"/>.</exception>
    [Pure]
    // Method looks longer than AggressiveInlining would usually support,
    // but when inlined for a given T, the unnecessary branches can
    // be culled to result in a relatively small amount of asm.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T, E> Ok<T, E>([DisallowNull] in T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        // Checking for value type first helps the JIT avoid running any caching checks for ref types
        if (typeof(T).IsValueType)
        {
            // Only the relevant branches are kept for value-type generic instantiations
            if (typeof(T) == typeof(Unit))
            {
                // Unit only has one possible value
                // Unsafe.As avoids dynamic type checks from casting since we know T is Unit
                return Unsafe.As<Result<T, E>>(Cache<E>.OkUnit);
            }

            if (typeof(T) == typeof(bool))
            {
                // Cache both true and false
                // Can't Unsafe.As a generic T into a bool as only ref types are supported
                var val = (bool)(object)value;
                var result = val ? Cache<E>.OkTrue : Cache<E>.OkFalse;
                return Unsafe.As<Result<T, E>>(result);
            }

            if (typeof(T) == typeof(int))
            {
                // Only cache the int 0
                var val = (int)(object)value;
                if (val == 0)
                    return Unsafe.As<Result<T, E>>(Cache<E>.OkIntZero);
            }

            if (typeof(T) == typeof(Guid))
            {
                // Only cache the empty guid
                var val = (Guid)(object)value;
                if (val == Guid.Empty)
                    return Unsafe.As<Result<T, E>>(Cache<E>.OkGuidEmpty);
            }
        }

        return new(value);
    }

    /// <summary>
    ///     Creates an <c>Error</c> <see cref="Result{T, E}"/>.
    /// </summary>
    /// <typeparam name="T">The type of value the result encapsulates.</typeparam>
    /// <typeparam name="E">The type of error the result encapsulates.</typeparam>
    /// <param name="error">The <typeparamref name="E"/> value to encapsulate.</param>
    /// <returns>An <c>Error(<typeparamref name="E"/>)</c> <see cref="Result{T, E}"/> which wraps <paramref name="error"/>.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="error"/> is <see langword="null"/>.</exception>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T, E> Error<T, E>(in E error)
    {
        ArgumentNullException.ThrowIfNull(error);

        return new(error);
    }
}
