using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Tyne;

public readonly partial struct Option<T>
{
    /// <summary>
    ///		Determines whether the specified <paramref name="obj"/> is an <see cref="Option{T}"/>, and if so is equal to the current instance.
    /// </summary>
    /// <param name="obj">An <see cref="object"/> to compare with this instance.</param>
    /// <returns>
    ///		<see langword="true"/> if the specified <paramref name="obj"/> is an <see cref="Option{T}"/>,
    ///		and is equal to this instance; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         Equality is determined with the following logic:
    ///         <list type="bullet">
    ///             <item>if <paramref name="obj"/> is an <see cref="Option{T}"/>, then use <see cref="Equals(in Option{T})"/></item>
    ///             <item>if <paramref name="obj"/> is a <typeparamref name="T"/>, then use <see cref="Equals(T)"/></item>
    ///             <item><see langword="true"/> if <paramref name="obj"/> is <see langword="null"/>, and this option does not have a value</item>
    ///             <item>otherwise; <see langword="false"/></item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///         <typeparamref name="T"/> equality is determined via <see cref="EqualityComparer{T}.Default"/>.
    ///     </para>
    /// </remarks>
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is Option<T> other)
            return Equals(in other);

        if (obj is T tValue)
            return Equals(tValue);

        // obj is null
        return false;
    }

    /// <summary>
    ///		Determines whether the specified <see cref="Option{T}"/> <paramref name="other"/> is equal to the current instance of <see cref="Option{T}"/>.
    /// </summary>
    /// <param name="other">The other <see cref="Option{T}"/> to compare with this instance.</param>
    /// <returns>
    ///		<see langword="true"/> if the specified <paramref name="other"/> is equal to this instance; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         Equality is determined with the following logic:
    ///         <list type="bullet">
    ///             <item><see langword="true"/> if both are <c>None()</c></item>
    ///             <item><see langword="true"/> if both are <c>Some(<typeparamref name="T"/>)</c> and their <typeparamref name="T"/> values are equal</item>
    ///             <item>otherwise; <see langword="false"/></item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///         <typeparamref name="T"/> equality is determined via <see cref="EqualityComparer{T}.Default"/>.
    ///     </para>
    /// </remarks>
    [Pure]
    public bool Equals(in Option<T> other)
    {
        // Both are Some(T), compare T values
        if (_hasValue && other._hasValue)
            return EqualityComparer<T>.Default.Equals(_value, other._value);

        // Otherwise, check if both are None()
        return !_hasValue && !other._hasValue;
    }

    /// <summary>
    ///		Determines whether the specified <see cref="Option{T}"/> <paramref name="other"/> is equal to the current instance of <see cref="Option{T}"/>.
    /// </summary>
    /// <param name="other">The other <see cref="Option{T}"/> to compare with this instance.</param>
    /// <returns>
    ///		<see langword="true"/> if the specified <paramref name="other"/> is equal to this instance; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         Equality is determined with the following logic:
    ///         <list type="bullet">
    ///             <item><see langword="true"/> if both are <c>None()</c></item>
    ///             <item><see langword="true"/> if one is <c>None()</c>, and the other is <see langword="null"/></item>
    ///             <item><see langword="true"/> if both are <c>Some(<typeparamref name="T"/>)</c> and their <typeparamref name="T"/> values are equal</item>
    ///             <item>otherwise; <see langword="false"/></item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///         <typeparamref name="T"/> equality is determined via <see cref="EqualityComparer{T}.Default"/>.
    ///     </para>
    /// </remarks>
    [Pure]
    public bool Equals(in Option<T>? other)
    {
        // If this is Some(T), check if the other is Some(T) and defer to Equals
        if (_hasValue)
            return other is { } option && Equals(option);

        // This is None, other is null, which is not equal
        if (other is null)
            return false;

        // This is None, other is not null, defer to Equals
        return Equals(other.Value);
    }

    /// <summary>
    ///     Determines whether the specified <typeparamref name="T"/>  <paramref name="other"/> is equal to the current instance of <see cref="Option{T}"/>.
    /// </summary>
    /// <param name="other">The other <typeparamref name="T"/> to compare with this instance.</param>
    /// <returns>
    ///		<see langword="true"/> if the specified <paramref name="other"/> is equal to this instance; otherwise, <see langword="false"/>.
    ///	</returns>
    ///	<remarks>
    ///     <para>
    ///         Equality is determined with the following logic:
    ///         <list type="bullet">
    ///             <item><see langword="true"/> if this is <c>None()</c> and <paramref name="other"/> is <see langword="null"/></item>
    ///             <item><see langword="true"/> if this is <c>Some(<typeparamref name="T"/>)</c> whose <typeparamref name="T"/> values equals <paramref name="other"/></item>
    ///             <item>otherwise; <see langword="false"/></item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///         <typeparamref name="T"/> equality is determined via <see cref="EqualityComparer{T}.Default"/>.
    ///     </para>
    /// </remarks>
    [Pure]
    public bool Equals(T? other)
    {
        // This is Some(T)
        if (_hasValue)
        {
            // Other can't be null
            if (other is null)
                return false;

            // Check if T value is equal to other
            return EqualityComparer<T>.Default.Equals(_value, other);
        }

        // This is None(), check if value is null
        return other is null;
    }

    /// <remarks>
    ///     You should use <see cref="Equals(in Option{T})"/> instead.
    /// </remarks>
    /// <inheritdoc cref="Equals(in Option{T})" />
    // This uses explicit interface implementation to guide callers to use the better Equals() overload instead
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool IEquatable<Option<T>>.Equals(Option<T> other) =>
        Equals(in other);

    /// <summary>
    ///		Determines whether <paramref name="left"/> is equal to <paramref name="right"/>.
    /// </summary>
    /// <param name="left">The left-hand <see cref="Option{T}"/>.</param>
    /// <param name="right">The right-hand <see cref="Option{T}"/>.</param>
    /// <returns>
    ///		<see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <see cref="Equals(in Option{T})"/> for how <see cref="Option{T}"/> equality is calculated.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(in Option<T> left, in Option<T> right) =>
        left.Equals(in right);

    /// <summary>
    ///		Determines whether <paramref name="left"/> is not equal to <paramref name="right"/>.
    /// </summary>
    /// <param name="left">The left-hand <see cref="Option{T}"/>.</param>
    /// <param name="right">The right-hand <see cref="Option{T}"/>.</param>
    /// <returns>
    ///		<see langword="false"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="true"/>.
    /// </returns>
    /// <remarks>
    ///     <see cref="Equals(in Option{T})"/> for how <see cref="Option{T}"/> equality is calculated.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(in Option<T> left, in Option<T> right) =>
        !(left == right);

    /// <summary>
    ///		Determines whether <paramref name="left"/> is equal to <paramref name="right"/>.
    /// </summary>
    /// <param name="left">The left-hand <see cref="Option{T}"/>.</param>
    /// <param name="right">The right-hand <typeparamref name="T"/>.</param>
    /// <returns>
    ///		<see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <see cref="Equals(T)"/> for how equality is calculated.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(in Option<T> left, T? right) =>
        left.Equals(right);

    /// <summary>
    ///		Determines whether <paramref name="left"/> is not equal to <paramref name="right"/>.
    /// </summary>
    /// <param name="left">The left-hand <see cref="Option{T}"/>.</param>
    /// <param name="right">The right-hand <typeparamref name="T"/>.</param>
    /// <returns>
    ///		<see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <see cref="Equals(T)"/> for how equality is calculated.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(in Option<T> left, T? right) =>
        !(left == right);

    /// <summary>
    ///		Determines whether <paramref name="left"/> is equal to <paramref name="right"/>.
    /// </summary>
    /// <param name="left">The left-hand <typeparamref name="T"/>.</param>
    /// <param name="right">The right-hand <see cref="Option{T}"/>.</param>
    /// <returns>
    ///		<see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <see cref="Equals(T)"/> for how equality is calculated.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(T? left, in Option<T> right) =>
        right.Equals(left);

    /// <summary>
    ///		Determines whether <paramref name="left"/> is not equal to <paramref name="right"/>.
    /// </summary>
    /// <param name="left">The left-hand <typeparamref name="T"/>.</param>
    /// <param name="right">The right-hand <see cref="Option{T}"/>.</param>
    /// <returns>
    ///		<see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <see cref="Equals(T)"/> for how equality is calculated.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(T? left, in Option<T> right) =>
        !(right == left);
}
