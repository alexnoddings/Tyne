using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Tyne;

/// <summary>
///     Static methods for creating <see cref="Option{T}"/>s.
/// </summary>
/// <seealso cref="Option{T}"/>
public static partial class Option
{
    /// <summary>
    ///     Creates a <c>None</c> <see cref="Option{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of value the option encapsulates.</typeparam>
    /// <returns>A <see langword="ref"/> <see langword="readonly"/> <c>None</c> <see cref="Option{T}"/>.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref readonly Option<T> None<T>() =>
        ref Cache<T>.None;

    /// <summary>
    ///     Creates a <c>Some(<typeparamref name="T"/>)</c> <see cref="Option{T}"/> using <paramref name="value"/>.
    /// </summary>
    /// <typeparam name="T">The type of value the option encapsulates.</typeparam>
    /// <param name="value">The <typeparamref name="T"/> to wrap.</param>
    /// <returns>A <c>Some(<typeparamref name="T"/>)</c> <see cref="Option{T}"/> which wraps <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">
    ///     When <paramref name="value"/> is <see langword="null"/>.
    ///     Use <see cref="From{T}"/> if <paramref name="value"/> may be <see langword="null"/>.
    /// </exception>
    [Pure]
    // Method looks longer than AggressiveInlining would usually support,
    // but when inlined for a given T, the unnecessary branches can
    // be culled to result in a relatively small amount of asm.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Some<T>([DisallowNull] T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        // Checking for value type first helps the JIT avoid running any caching checks for ref types
        if (typeof(T).IsValueType)
        {
            // Only the relevant branches are kept for value-type generic instantiations
            if (typeof(T) == typeof(Unit))
            {
                // Unit only has one possible value
                // We can't use Unsafe.As to avoid dynamic type checking since it only works on reference types, not structs
                return (Option<T>)(object)Cache.SomeUnit;
            }

            if (typeof(T) == typeof(bool))
            {
                // Cache both true and false
                // Can't Unsafe.As a generic T into a bool as only ref types are supported
                var val = (bool)(object)value;
                var option = val ? Cache.SomeTrue : Cache.SomeFalse;
                return (Option<T>)(object)option;
            }

            if (typeof(T) == typeof(int))
            {
                // Only cache the int 0
                var val = (int)(object)value;
                if (val == 0)
                    return (Option<T>)(object)Cache.SomeIntZero;
            }
        }

        return new(value);
    }

    /// <summary>
    ///     Creates an <see cref="Option{T}"/> from <paramref name="value"/>.
    /// </summary>
    /// <typeparam name="T">The type of value the option encapsulates.</typeparam>
    /// <param name="value">The <typeparamref name="T"/> to wrap.</param>
    /// <returns>
    ///     A <c>Some(<typeparamref name="T"/>)</c> <see cref="Option{T}"/> if <paramref name="value"/> is not <see langword="null"/>;
    /// otherwise, a <c>None</c> <see cref="Option{T}"/>.
    /// </returns>
    /// <remarks>
    ///     Bear in mind that value-typed <typeparamref name="T"/>s will always return <c>Some(<typeparamref name="T"/>)</c> as <typeparamref name="T"/> cannot be <see langword="null"/>.
    /// </remarks>
    [Pure]
    public static Option<T> From<T>(T? value) =>
        value is null
            ? None<T>()
            : Some(value);
}
