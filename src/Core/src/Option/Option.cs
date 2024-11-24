using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Tyne;

/// <summary>
///     Static methods for creating <see cref="Option{T}"/>s.
/// </summary>
public static class Option
{
    /// <summary>
    ///     Creates a <c>None</c> <see cref="Option{T}"/> (i.e. <see cref="Option{T}.HasValue"/> is <see langword="false"/>).
    /// </summary>
    /// <typeparam name="T">The type of value the option encapsulates.</typeparam>
    /// <returns>A <see langword="ref"/> <see langword="readonly"/> <c>None</c> <see cref="Option{T}"/>.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref readonly Option<T> None<T>() =>
        ref Option<T>.None;

    /// <summary>
    ///     Creates a <c>Some(<typeparamref name="T"/>)</c> <see cref="Option{T}"/> using <paramref name="value"/>.
    /// </summary>
    /// <typeparam name="T">The type of value the option encapsulates.</typeparam>
    /// <param name="value">The <typeparamref name="T"/> to wrap.</param>
    /// <returns>A <c>Some(<typeparamref name="T"/>)</c> <see cref="Option{T}"/> which wraps <paramref name="value"/>.</returns>
    /// <remarks>
    ///     A <see cref="BadOptionException"/> will be thrown if <paramref name="value"/> is <see langword="null"/>.
    ///     If you are unsure if <paramref name="value"/> is <c>Some(<typeparamref name="T"/>)</c> or <c>None</c>,
    ///     consider calling <see cref="From{T}(T)"/> instead.
    /// </remarks>
    /// <exception cref="BadOptionException">When <paramref name="value"/> is <see langword="null"/>.</exception>
    [Pure]
    // Method looks longer than AggressiveInlining would usually support,
    // but when inlined for a given T, the unnecessary branches can
    // be culled to result in a relatively small amount of asm.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Some<T>(T value)
    {
        if (value is null)
            throw new BadOptionException(ExceptionMessages.Option_SomeMustHaveValue);

        // Checking for value type first helps the JIT avoid running any caching checks for ref types
        if (typeof(T).IsValueType)
        {
            // Only the relevant branches are kept for value-type generic instantiations
            if (typeof(T) == typeof(Unit))
            {
                // Unit only has one possible value.
                return (Option<T>)(object)Cache.SomeUnit;
            }

            if (typeof(T) == typeof(bool))
            {
                // Cache both true and false bools.
                // Can't Unsafe.As a generic T into a bool as only ref types are supported.
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
    /// <param name="value">The <typeparamref name="T"/> to potentially wrap.</param>
    /// <returns>An <see cref="Option{T}"/> which may be <c>Some(<typeparamref name="T"/>)</c> or <c>None</c>, depending on <paramref name="value"/>.</returns>
    /// <remarks>
    ///     <para>
    ///         If <paramref name="value"/> is <see langword="null"/>, this will return <see cref="None{T}"/>.
    ///         Otherwise, this will return <see cref="Some{T}(T)"/>.
    ///     </para>
    ///     <para>
    ///         Bear in mind that value-typed <typeparamref name="T"/>s will always return <c>Some(<typeparamref name="T"/>)</c> as <typeparamref name="T"/> cannot be <see langword="null"/>.
    ///     </para>
    /// </remarks>
    [Pure]
    public static Option<T> From<T>(T? value) =>
        value is null
        ? Option<T>.None
        : new(value);

    /// <summary>
    ///     Caches common and simple option types.
    /// </summary>
    internal static class Cache
    {
        public static readonly Option<Unit> SomeUnit = new(Unit.Value);
        public static readonly Option<bool> SomeTrue = new(true);
        public static readonly Option<bool> SomeFalse = new(false);
        public static readonly Option<int> SomeIntZero = new(0);
    }
}
