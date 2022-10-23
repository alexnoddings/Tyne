using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Tyne;

/// <summary>
///		The result of an operation.
/// </summary>
/// <typeparam name="T">The value returned from a successful operation.</typeparam>
/// <typeparam name="E">The error returned from an unsuccessful operation.</typeparam>
// DebuggerDisplay produces either Ok({Value}) or Err({Error})
[DebuggerDisplay(@"{(IsOk ? ""Ok("" : ""Err(""),nq}{(IsOk ? (object)Value : Error)})")]
public class Result<T, E> : IEquatable<Result<T, E>>
{
	private readonly T? _value;
	private readonly E? _error;

	/// <summary>
	///		The <typeparamref name="T"/> value returned by a successful operation.
	/// </summary>
	/// <remarks>
	///		You should always ensure <see cref="IsOk"/> is <see langword="true" /> before attempting to access this.
	///		If the result <see cref="IsError"/>, accessing this will throw an <see cref="InvalidOperationException"/>.
	/// </remarks>
	/// <exception cref="InvalidOperationException"/>
	public T Value =>
		IsOk
		? _value!
		: throw new InvalidOperationException($"{nameof(Value)} cannot be accessed when {nameof(IsOk)} is false");

	/// <summary>
	///		The <typeparamref name="E"/> error returned by an unsuccessful operation.
	/// </summary>
	/// <remarks>
	///		You should always ensure <see cref="IsError"/> is <see langword="true" /> before attempting to access this.
	///		If the result <see cref="IsOk"/>, accessing this will throw an <see cref="InvalidOperationException"/>.
	/// </remarks>
	/// <exception cref="InvalidOperationException"/>
	public E Error =>
		!IsOk
		? _error!
		: throw new InvalidOperationException($"{nameof(Error)} cannot be accessed when {nameof(IsOk)} is true");

	/// <summary>
	///		<see langword="true"/> if the operation was successful, otherwise <see langword="false"/>.
	/// </summary>
	[Pure]
	public bool IsOk { get; }

	/// <summary>
	///		<see langword="false"/> if the operation was successful, otherwise <see langword="true"/>.
	/// </summary>
	/// <remarks>
	///		This is simply the negation of <see cref="IsOk"/>.
	///		It is provided as <c>result.IsError</c> is easier to comprehend than <c>!result.IsOk</c>.
	/// </remarks>
	[Pure]
	public bool IsError
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => !IsOk;
	}

	protected internal Result(bool isOk, T? value, E? error)
	{
		if (isOk && value is null)
			throw new ArgumentNullException(nameof(value), $"A {nameof(value)} should be passed when {nameof(isOk)} is true.");

		if (!isOk && error is null)
			throw new ArgumentNullException(nameof(error), $"An {nameof(error)} should be passed when {nameof(isOk)} is false.");

		// T or E may not be a nullable type (e.g. an int or enum), so we don't want to enforce
		// that value must be null when !isOk, or that error must be null when isOk.

		IsOk = isOk;
		_value = value;
		_error = error;
	}

	/// <summary>
	///		Determines whether the specified <paramref name="other"/> is equal to the current instance of the same type.
	/// </summary>
	/// <param name="other">The other <see cref="Result{T, E}"/> to compare with this instance.</param>
	/// <returns>
	///		<see langword="true"/> if the specified <paramref name="other"/> is equal to this instance; otherwise, <see langword="false"/>.
	/// </returns>
	/// <remarks>
	///		This relies on the <see cref="Value"/> or <see cref="Error"/> overriding <see cref="object.Equals(object?)"/>.
	///		If they do not, this may not behave as expected.
	/// </remarks>
	public virtual bool Equals(Result<T, E>? other)
	{
		if (other is null) return false;

		if (IsOk)
			return other.IsOk && _value!.Equals(other._value);

		return other.IsError && _error!.Equals(other._error);
	}

	/// <summary>
	///		Determines whether the specified <paramref name="obj"/> is a <see cref="Result{T, E}"/>, and is equal to the current instance.
	/// </summary>
	/// <param name="obj">An <see cref="object"/> to compare with this instance.</param>
	/// <returns>
	///		<see langword="true"/> if the specified <paramref name="obj"/> is a <see cref="Result{T, E}"/>, and is equal to this instance; otherwise, <see langword="false"/>.
	/// </returns>
	/// <remarks>
	///		This relies on the <see cref="Value"/> or <see cref="Error"/> overriding <see cref="object.Equals(object?)"/>.
	///		If they do not, this may not behave as expected.
	/// </remarks>
	public override bool Equals(object? obj) =>
		obj is Result<T, E> other && Equals(other);

	/// <summary>
	///		Determines whether the <paramref name="left"/> is equal to the <paramref name="right"/>.
	/// </summary>
	/// <param name="left">The left-hand <see cref="Result{T,E}"/>.</param>
	/// <param name="right">The right-hand <see cref="Result{T,E}"/>.</param>
	/// <returns>
	///		<see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	/// <remarks>
	///		See <see cref="Equals(Result{T, E}?)"/>.
	/// </remarks>
	public static bool operator ==(Result<T, E> left, Result<T, E> right) =>
		left.Equals(right);

	/// <summary>
	///		Determines whether the <paramref name="left"/> is not equal to the <paramref name="right"/>.
	/// </summary>
	/// <param name="left">The left-hand <see cref="Result{T,E}"/>.</param>
	/// <param name="right">The right-hand <see cref="Result{T,E}"/>.</param>
	/// <returns>
	///		<see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	/// <remarks>
	///		See <see cref="Equals(Result{T, E}?)"/>.
	///	</remarks>
	public static bool operator !=(Result<T, E> left, Result<T, E> right) =>
		!left.Equals(right);

	/// <summary>
	///		Returns a hash code for this instance.
	/// </summary>
	/// <returns>
	///		A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
	/// </returns>
	public override int GetHashCode() =>
		HashCode.Combine(IsOk, _value, _error);

	/// <summary>
	///		Returns a <see cref="string" /> that represents this instance.
	/// </summary>
	/// <returns>A <see cref="string" /> that represents this instance.</returns>
	public override string ToString() =>
		IsOk
		? $"Ok({_value})"
		: $"Err({_error})";

	/// <summary>
	///		Converts a <c>Result&lt;<typeparamref name="TIn"/>, <typeparamref name="EIn"/>&gt;</c> into a <c>Result&lt;<typeparamref name="T"/>, <typeparamref name="E"/>&gt;</c>.
	/// </summary>
	/// <typeparam name="TIn">The input ok type. This muust inherit from <typeparamref name="T"/>.</typeparam>
	/// <typeparam name="TIn">The input error type. This muust inherit from <typeparamref name="E"/>.</typeparam>
	/// <param name="result">The result to convert.</param>
	/// <returns>A <c>Result&lt;<typeparamref name="T"/>, <typeparamref name="E"/>&gt;</c> with the same value or error as <paramref name="result"/>.</returns>
	/// <remarks>
	///		This is used to convert the <paramref name="result"/> to a base type while retaining the value or error,
	///		such as converting <c>Result&lt;InheritedType, InheritedError&gt;</c> to <c>Result&lt;BaseType, BaseError&gt;</c>.
	/// </remarks>
	[Pure]
	public static Result<T, E> From<TIn, EIn>(Result<TIn, EIn> result) where TIn : T where EIn : E =>
		result.IsOk
		? Result.Ok<T, E>(result.Value)
		: Result.Err<T, E>(result.Error);
}
