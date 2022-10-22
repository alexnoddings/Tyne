using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Tyne;

/// <summary>
///     The <see cref="Unit"/> type is a type that indicates the absence of a specific value; the <see cref="Unit"/> type has only a single value, which acts as a placeholder when no other value exists or is needed. Represents <see cref="Void"/> since it is not a valid type in C#.
/// </summary>
/// <remarks>
///		Implementation based on <see href="https://github.com/jbogard/MediatR/blob/master/src/MediatR.Contracts/Unit.cs">MediatR's Unit</see>.
/// </remarks>
[DebuggerDisplay("Unit ()")]
[SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Parameters are needed for, but ignored by, comparison methods.")]
public readonly struct Unit : IEquatable<Unit>, IComparable<Unit>, IComparable
{
	private static readonly Unit _value = new();

	/// <summary>
	///		The default and only value of <see cref="Unit"/>.
	/// </summary>
	public static ref readonly Unit Value => ref _value;

	/// <summary>
	///		<para>
	///			Creates a new <see cref="Unit"/>.
	///		</para>
	///		<para>
	///			<b>Note:</b> You should use <see cref="Unit.Value"/> instead of constructing a new one.
	///		</para>
	/// </summary>
	[Obsolete($"Use {nameof(Value)} instead.", true, DiagnosticId = "TYNE001")]
	public Unit()
	{
		AsTask = Task.FromResult(this);
	}

	/// <summary>
	///		A <see cref="Task{TResult}"/> whose result is <see langword="this"/>.
	/// </summary>
	/// <remarks>
	///		This returns the same <see cref="Task{TResult}"/> instance every time.
	/// </remarks>
	public Task<Unit> AsTask { get; }

	/// <summary>
	///		Creates a <see cref="ValueTask{TResult}"/> whose result is <see langword="this"/>.
	/// </summary>
	/// <remarks>
	///		This returns a new <see cref="ValueTask{TResult}"/> instance every time.
	/// </remarks>
	public ValueTask<Unit> AsValueTask => ValueTask.FromResult(this);

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
