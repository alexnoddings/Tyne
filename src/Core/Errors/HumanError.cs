using System.Diagnostics;

namespace Tyne;

/// <summary>
///		A simple implementation of <see cref="IHumanError"/>.
/// </summary>
[DebuggerDisplay("{Message}")]
public class HumanError : IHumanError, IEquatable<HumanError>
{
	public string HumanErrorMessage { get; }

	/// <summary>
	///		Creates a new <see cref="HumanError"/> with <paramref name="message"/> as the <see cref="HumanErrorMessage"/>.
	/// </summary>
	/// <param name="message">The <see cref="HumanErrorMessage"/>.</param>
	/// <exception cref="ArgumentNullException">When <paramref name="message"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentException">When <paramref name="message"/> is empty or whitespace.</exception>
	public HumanError(string message)
	{
		if (message is null)
			throw new ArgumentNullException(nameof(message));

		if (string.IsNullOrWhiteSpace(message))
			throw new ArgumentException("Message cannot be empty or whitespace.", nameof(message));

		HumanErrorMessage = message;
	}

	/// <summary>
	///		Determines whether the specified <paramref name="other"/> is equal to the current instance of the same type.
	/// </summary>
	/// <param name="other">The other <see cref="HumanError"/> to compare with this instance.</param>
	/// <returns>
	///		<see langword="true"/> if the specified <paramref name="other"/> is equal to this instance; otherwise, <see langword="false"/>.
	/// </returns>
	/// <remarks>
	///		This checks that the two <see cref="HumanErrorMessage"/>s are equal.
	/// </remarks>
	public virtual bool Equals(HumanError? other) =>
		other is not null
		&& other.HumanErrorMessage.Equals(HumanErrorMessage);

	/// <summary>
	///		Determines whether the specified <paramref name="obj"/> is a <see cref="HumanError"/>, and is equal to the current instance.
	/// </summary>
	/// <param name="obj">An <see cref="object"/> to compare with this instance.</param>
	/// <returns>
	///		<see langword="true"/> if the specified <paramref name="obj"/> is a <see cref="HumanError"/>, and is equal to this instance; otherwise, <see langword="false"/>.
	/// </returns>
	/// <remarks>
	///		This checks that the two <see cref="HumanErrorMessage"/>s are equal.
	/// </remarks>
	public override bool Equals(object? obj) =>
		obj is HumanError error
		&& Equals(error);

	/// <summary>
	///		Determines whether the <paramref name="left"/> is equal to the <paramref name="right"/>.
	/// </summary>
	/// <param name="left">The left-hand <see cref="HumanError"/>.</param>
	/// <param name="right">The right-hand <see cref="HumanError"/>.</param>
	/// <returns>
	///		<see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	/// <remarks>
	///		See <see cref="Equals(HumanError?)"/>.
	/// </remarks>
	public static bool operator ==(HumanError left, HumanError right) => 
		left.Equals(right);

	/// <summary>
	///		Determines whether the <paramref name="left"/> is not equal to the <paramref name="right"/>.
	/// </summary>
	/// <param name="left">The left-hand <see cref="HumanError"/>.</param>
	/// <param name="right">The right-hand <see cref="HumanError"/>.</param>
	/// <returns>
	///		<see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	/// <remarks>
	///		See <see cref="Equals(HumanError?)"/>.
	///	</remarks>
	public static bool operator !=(HumanError left, HumanError right) => 
		!left.Equals(right);

	/// <summary>
	///		Returns a hash code for this instance.
	/// </summary>
	/// <returns>
	///		A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
	/// </returns>
	public override int GetHashCode() =>
		HumanErrorMessage.GetHashCode();
}
