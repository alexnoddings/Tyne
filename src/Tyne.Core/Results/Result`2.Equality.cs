using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Tyne;

public sealed partial class Result<T, TE>
{
    /// <summary>
    ///		Determines whether the specified <paramref name="obj"/> is a <see cref="Result{T, TE}"/>, and if so is equal to the current instance.
    /// </summary>
    /// <param name="obj">An <see cref="object"/> to compare with this instance.</param>
    /// <returns>
    ///		<see langword="true"/> if the specified <paramref name="obj"/> is a <see cref="Result{T, TE}"/>,
    ///		and is equal to this instance; otherwise, <see langword="false"/>.
    /// </returns>
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is Result<T, TE> other)
            return Equals(other);

        return false;
    }

    /// <summary>
    ///		Determines whether the specified <paramref name="other"/> is equal to the current instance of the same type.
    /// </summary>
    /// <param name="other">The other <see cref="Result{T, TE}"/> to compare with this instance.</param>
    /// <returns>
    ///		<see langword="true"/> if the specified <paramref name="other"/> is equal to this instance; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         Two <see cref="Result{T, TE}"/>s are equal if either:
    ///         <list type="bullet">
    ///             <item>both are <c>Ok(<typeparamref name="T"/>)</c> and their <typeparamref name="T"/> values are equal</item>
    ///             <item>both are <c>Error(<typeparamref name="TE"/>)</c> and their <typeparamref name="TE"/> errors are equal</item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///         Equality is determined via <see cref="EqualityComparer{T}.Default"/>.
    ///     </para>
    /// </remarks>
    [Pure]
    public bool Equals([NotNullWhen(true)] Result<T, TE>? other)
    {
        if (other is null)
            return false;

        // Both are Ok(T), compare T values
        if (_isOk && other._isOk)
            return EqualityComparer<T>.Default.Equals(_value, other._value);

        // Only one is Ok(T), then they cannot be equal
        if (_isOk || other._isOk)
            return false;

        // Neither are Ok(T), compare Errors
        return EqualityComparer<TE>.Default.Equals(_error, other._error);
    }

    /// <summary>
    ///		Determines whether the <paramref name="left"/> is equal to the <paramref name="right"/>.
    /// </summary>
    /// <param name="left">The left-hand <see cref="Result{T, TE}"/>.</param>
    /// <param name="right">The right-hand <see cref="Result{T, TE}"/>.</param>
    /// <returns>
    ///		<see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <see cref="Equals(Result{T, TE})"/> for how <see cref="Result{T, TE}"/> equality is calculated.
    /// </remarks>
    [Pure]
    public static bool operator ==(in Result<T, TE>? left, in Result<T, TE>? right)
    {
        if (left is null && right is null)
            return true;

        if (left is not null && right is not null)
            return left.Equals(right);

        return false;
    }

    /// <summary>
    ///		Determines whether the <paramref name="left"/> is not equal to the <paramref name="right"/>.
    /// </summary>
    /// <param name="left">The left-hand <see cref="Result{T, TE}"/>.</param>
    /// <param name="right">The right-hand <see cref="Result{T, TE}"/>.</param>
    /// <returns>
    ///		<see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <see cref="Equals(Result{T, TE})"/> for how <see cref="Result{T, TE}"/> equality is calculated.
    ///	</remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(in Result<T, TE>? left, in Result<T, TE>? right) =>
        !(left == right);
}
