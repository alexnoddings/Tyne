using System.Diagnostics;

namespace Tyne;

/// <summary>
///     The <see cref="Unit"/> type is a type that indicates the absence of a specific value;
///     the <see cref="Unit"/> type has only a single value, which acts as a placeholder when no other value exists or is needed.
///     Represents <see cref="void"/> since that is not a valid <see cref="Type"/> in C#.
/// </summary>
// Implementation heavily inspired by MediatR's Unit implementation.
// https://github.com/jbogard/MediatR/blob/master/src/MediatR.Contracts/Unit.cs
[DebuggerDisplay("()")]
public readonly struct Unit : IEquatable<Unit>, IComparable<Unit>, IComparable
{
    private static readonly Unit _value = new();

    /// <summary>
    ///		The default and only value of <see cref="Unit"/>.
    /// </summary>
    public static ref readonly Unit Value => ref _value;

    /// <summary>
    ///		Creates a new <see cref="Unit"/>.
    /// </summary>
    /// <remarks>
    ///		You should use <see cref="Value"/> instead of constructing a new <see cref="Unit"/>.
    /// </remarks>
    public Unit()
    {
    }

    /// <summary>
    ///		A <see cref="Task{TResult}"/> whose result is <see cref="Value"/>.
    /// </summary>
    public static Task<Unit> AsTask { get; } = Task.FromResult(_value);

    /// <summary>
    ///		Creates a <see cref="ValueTask{TResult}"/> whose result is <see cref="Value"/>.
    /// </summary>
    public static ValueTask<Unit> AsValueTask => ValueTask.FromResult(_value);

    /// <summary>
    ///		Determines whether the specified <paramref name="other"/> is equal to the current instance of the same type.
    /// </summary>
    /// <param name="other">The other <see cref="Unit"/> to compare with this instance.</param>
    /// <remarks>This always returns <see langword="true"/>.</remarks>
    public bool Equals(Unit other) => true;

    /// <summary>
    ///		Determines whether the specified <paramref name="obj"/> is equal to the current instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>
    ///		<see langword="true"/> if the specified <paramref name="obj"/> is a <see cref="Unit"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>This is always <see langword="true"/> if <paramref name="obj"/> is a <see cref="Unit"/>, and <see langword="false"/> otherwise.</remarks>
    public override bool Equals(object? obj) => obj is Unit;

    /// <summary>
    ///		Determines whether <see cref="Unit"/>s are equal.
    /// </summary>
    /// <param name="_1">The left-hand <see cref="Unit"/>. This is ignored.</param>
    /// <param name="_2">The right-hand <see cref="Unit"/>. This is ignored.</param>
    /// <returns><see langword="true"/></returns>
    /// <remarks>This always returns <see langword="true"/>.</remarks>
    public static bool operator ==(Unit _1, Unit _2) => true;

    /// <summary>
    ///		Determines whether <see cref="Unit"/>s are not equal.
    /// </summary>
    /// <param name="_1">The left-hand <see cref="Unit"/>. This is ignored.</param>
    /// <param name="_2">The right-hand <see cref="Unit"/>. This is ignored.</param>
    /// <returns><see langword="false"/></returns>
    /// <remarks>This always returns <see langword="false"/>.</remarks>
    public static bool operator !=(Unit _1, Unit _2) => false;

    /// <summary>
    ///		Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    ///		A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
    /// </returns>
    /// <remarks>This is always <c>0</c> for a <see cref="Unit"/>.</remarks>
    public override int GetHashCode() => 0;

    /// <inheritdoc />
    /// <param name="_">The other <see cref="Unit"/>. This is ignored.</param>
    /// <remarks>This is always <c>0</c>.</remarks>
    public int CompareTo(Unit _) => 0;

    /// <inheritdoc />
    /// <param name="obj">The other <see cref="object"/>. This is ignored.</param>
    /// <remarks>This is always <c>0</c>.</remarks>
    public int CompareTo(object? obj) => 0;

    /// <summary>
    ///		Determines whether one <see cref="Unit"/>s is less than another <see cref="Unit"/>.
    /// </summary>
    /// <param name="_1">The left-hand <see cref="Unit"/>. This is ignored.</param>
    /// <param name="_2">The right-hand <see cref="Unit"/>. This is ignored.</param>
    /// <returns><see langword="false"/></returns>
    /// <remarks>This always returns <see langword="false"/>.</remarks>
    public static bool operator <(Unit _1, Unit _2) => false;

    /// <summary>
    ///		Determines whether one <see cref="Unit"/>s is less than or equal to another <see cref="Unit"/>.
    /// </summary>
    /// <param name="_1">The left-hand <see cref="Unit"/>. This is ignored.</param>
    /// <param name="_2">The right-hand <see cref="Unit"/>. This is ignored.</param>
    /// <returns><see langword="true"/></returns>
    /// <remarks>This always returns <see langword="true"/>.</remarks>
    public static bool operator <=(Unit _1, Unit _2) => true;

    /// <summary>
    ///		Determines whether one <see cref="Unit"/>s is greater than another <see cref="Unit"/>.
    /// </summary>
    /// <param name="_1">The left-hand <see cref="Unit"/>. This is ignored.</param>
    /// <param name="_2">The right-hand <see cref="Unit"/>. This is ignored.</param>
    /// <returns><see langword="false"/></returns>
    /// <remarks>This always returns <see langword="false"/>.</remarks>
    public static bool operator >(Unit _1, Unit _2) => false;

    /// <summary>
    ///		Determines whether one <see cref="Unit"/>s is greater than or equal to another <see cref="Unit"/>.
    /// </summary>
    /// <param name="_1">The left-hand <see cref="Unit"/>. This is ignored.</param>
    /// <param name="_2">The right-hand <see cref="Unit"/>. This is ignored.</param>
    /// <returns><see langword="true"/></returns>
    /// <remarks>This always returns <see langword="true"/>.</remarks>
    public static bool operator >=(Unit _1, Unit _2) => true;

    /// <summary>
    ///		Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    /// <remarks>This always returns <c>()</c>.</remarks>
    public override string ToString() => "()";
}
