using System.Diagnostics;

namespace Tyne;

/// <summary>
///     The <see cref="Unit"/> type is a type that indicates the absence of a specific value; the <see cref="Unit"/> type has only a single value, which acts as a placeholder when no other value exists or is needed. Represents <see cref="Void"/> since it is not a valid type in C#.
/// </summary>
/// <remarks>
///		Implementation based on <see href="https://github.com/jbogard/MediatR/blob/master/src/MediatR.Contracts/Unit.cs">MediatR's Unit</see>.
/// </remarks>
[DebuggerDisplay("Unit ()")]
public readonly struct Unit : IEquatable<Unit>, IComparable<Unit>, IComparable
{
	private static readonly Unit _value = new();

	/// <summary>
	///		The default and only value of <see cref="Unit"/>.
	/// </summary>
	public static ref readonly Unit Value => ref _value;

	[Obsolete($"Use {nameof(Unit.Value)} instead.", DiagnosticId = "TYN001")]
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
	///		Determines whether the specified <paramref name="obj"/> is equal to the current instance of the same type.
	/// </summary>
	/// <param name="obj">The object to compare with this instance.</param>
	/// <returns>
	///		<see langword="true"/> if the specified <paramref name="obj"/> is equal to this instance; otherwise, <see langword="false"/>.
	/// </returns>
	/// <remarks>This is always <see langword="true"/> for a <see cref="Unit"/>.</remarks>
	public bool Equals(Unit obj) => true;

	/// <summary>
	///		Determines whether the specified <paramref name="obj"/> is equal to the current instance.
	/// </summary>
	/// <param name="obj">The object to compare with the current instance.</param>
	/// <returns>
	///		<see langword="true"/> if the specified <paramref name="obj"/> is equal to this instance; otherwise, <see langword="false"/>.
	/// </returns>
	/// <remarks>This is always <see langword="true"/> if <paramref name="obj"/> is a <see cref="Unit"/>.</remarks>
	public override bool Equals(object? obj) => obj is Unit;

	/// <summary>
	///		Determines whether the <paramref name="left"/> is equal to the <paramref name="right"/>.
	/// </summary>
	/// <param name="left">The left-hand <see cref="Unit"/>.</param>
	/// <param name="right">The right-hand <see cref="Unit"/>.</param>
	/// <returns>
	///		<see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	/// <remarks>This is always <see langword="true"/> for a <see cref="Unit"/>.</remarks>
	public static bool operator ==(Unit left, Unit right) => true;

	/// <summary>
	///		Determines whether the <paramref name="left"/> is not equal to the <paramref name="right"/>.
	/// </summary>
	/// <param name="left">The left-hand <see cref="Unit"/>.</param>
	/// <param name="right">The right-hand <see cref="Unit"/>.</param>
	/// <returns>
	///		<see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	/// <remarks>This is always <see langword="false"/> for a <see cref="Unit"/>.</remarks>
	public static bool operator !=(Unit left, Unit right) => false;

	/// <summary>
	///		Returns a hash code for this instance.
	/// </summary>
	/// <returns>
	///		A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
	/// </returns>
	/// <remarks>This is always <c>0</c> for a <see cref="Unit"/>.</remarks>
	public override int GetHashCode() => 0;

	/// <inheritdoc />
	/// <remarks>This is always <c>0</c>.</remarks>
	public int CompareTo(Unit other) => 0;

	/// <inheritdoc />
	/// <remarks>This is always <c>0</c>.</remarks>
	public int CompareTo(object? obj) => 0;

	/// <summary>
	///		Determines whether the <paramref name="left"/> is less than the <paramref name="right"/>.
	/// </summary>
	/// <param name="left">The left-hand <see cref="Unit"/>.</param>
	/// <param name="right">The right-hand <see cref="Unit"/>.</param>
	/// <returns>
	///		<see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	/// <remarks>This is always <see langword="false"/> for a <see cref="Unit"/>.</remarks>
	public static bool operator <(Unit left, Unit right) => false;

	/// <summary>
	///		Determines whether the <paramref name="left"/> is less than or equal to the <paramref name="right"/>.
	/// </summary>
	/// <param name="left">The left-hand <see cref="Unit"/>.</param>
	/// <param name="right">The right-hand <see cref="Unit"/>.</param>
	/// <returns>
	///		<see langword="true"/> if <paramref name="left"/> is less or equal to than <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	/// <remarks>This is always <see langword="true"/> for a <see cref="Unit"/>.</remarks>
	public static bool operator <=(Unit left, Unit right) => true;

	/// <summary>
	///		Determines whether the <paramref name="left"/> is greater than the <paramref name="right"/>.
	/// </summary>
	/// <param name="left">The left-hand <see cref="Unit"/>.</param>
	/// <param name="right">The right-hand <see cref="Unit"/>.</param>
	/// <returns>
	///		<see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	/// <remarks>This is always <see langword="false"/> for a <see cref="Unit"/>.</remarks>
	public static bool operator >(Unit left, Unit right) => false;

	/// <summary>
	///		Determines whether the <paramref name="left"/> is greater than or equal to the <paramref name="right"/>.
	/// </summary>
	/// <param name="left">The left-hand <see cref="Unit"/>.</param>
	/// <param name="right">The right-hand <see cref="Unit"/>.</param>
	/// <returns>
	///		<see langword="true"/> if <paramref name="left"/> is greater or equal to than <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	/// <remarks>This is always <see langword="true"/> for a <see cref="Unit"/>.</remarks>
	public static bool operator >=(Unit left, Unit right) => true;

	/// <summary>
	///		Returns a <see cref="string" /> that represents this instance.
	/// </summary>
	/// <returns>A <see cref="string" /> that represents this instance.</returns>
	/// <remarks>This always returns <c>()</c> for a <see cref="Unit"/>.</remarks>
	public override string ToString() => "()";
}
