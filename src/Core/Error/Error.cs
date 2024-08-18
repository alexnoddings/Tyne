using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Tyne;

/// <summary>
///     Represents an error which has occurred.
/// </summary>
/// <remarks>
///     <para>
///         This is designed to provide stronger error capabilities than <see cref="Exception"/>s,
///         and also focuses on handling the bad path rather than
///         letting exceptions bubble up the app to be handled later.
///     </para>
///     <para>
///         Normally, an error comprises a <see cref="Code"/> and a <see cref="Message"/>.
///         The code indicates the type of error (e.g. a validation error), while the message
///         gives more detailed information. Messages should be user-friendly.
///     </para>
///     <para>
///         An error can also have been <see cref="CausedBy"/> an <see cref="Exception"/>.
///         This is used to bridge the gap between <see cref="Exception"/>s and <see cref="Error"/>s.
///     </para>
/// </remarks>
/// <seealso cref="ErrorExtensions" />
/// <seealso cref="ErrorJsonConverter" />
[DebuggerDisplay("{ToString(),nq}")]
[DebuggerTypeProxy(typeof(DebuggerTypeProxy))]
[JsonConverter(typeof(ErrorJsonConverter))]
public class Error : IEquatable<Error>
{
    internal const string DefaultCode = "default";
    internal const string DefaultMessage = "Unknown error.";

    /// <summary>
    ///     The default instance of <see cref="Error"/>.
    /// </summary>
    /// <remarks>
    ///     This should be avoided unless you have no other context for creating an <see cref="Error"/>.
    /// </remarks>
    [Pure]
    public static Error Default { get; } = new(DefaultCode, DefaultMessage, null);

    /// <summary>
    ///     The error's code.
    /// </summary>
    /// <remarks>
    ///     <para>This is never <see langword="null"/>, empty, or whitespace.</para>
    ///     <para>Codes are case-insensitive when <see cref="Error"/>s are compared.</para>
    /// </remarks>
    [Pure]
    public string Code { get; }

    /// <summary>
    ///     The error's message.
    /// </summary>
    /// <remarks>
    ///     <para>This is never <see langword="null"/>, empty, or whitespace.</para>
    ///     <para>Messages are case-sensitive when <see cref="Error"/>s are compared.</para>
    /// </remarks>
    [Pure]
    public string Message { get; }

    /// <summary>
    ///     Optionally, an <see cref="Exception"/> which caused this <see cref="Error"/>.
    /// </summary>
    /// <remarks>
    ///     This is designed to bridge the exception/error gap.
    /// </remarks>
    [Pure]
    public Option<Exception> CausedBy { get; }

    /// <summary>
    ///     Creates an <see cref="Error"/> with all properties populated.
    /// </summary>
    /// <param name="code">The code to create the <see cref="Error"/> with.</param>
    /// <param name="message">The message to create the <see cref="Error"/> with.</param>
    /// <param name="causedBy">The <see cref="Exception"/> which caused this <see cref="Error"/>.</param>
    /// <remarks>
    ///     Invalid codes (<see cref="Internal.Error.IsValidCode(string?)"/>) and messages (<see cref="Internal.Error.IsValidMessage(string?)"/>) are replaced with defaults.
    /// </remarks>
    protected internal Error(string? code, string? message, Exception? causedBy)
    {
        Code = Internal.Error.CodeOrDefault(code);
        Message = Internal.Error.MessageOrDefault(message);
        CausedBy = causedBy;
    }

    /// <summary>
    ///     How <see cref="Code"/>s should be compared.
    /// </summary>
    protected static readonly StringComparison CodeComparisonType = StringComparison.OrdinalIgnoreCase;

    /// <summary>
    ///     How <see cref="Message"/>s should be compared.
    /// </summary>
    protected static readonly StringComparison MessageComparisonType = StringComparison.Ordinal;

    /// <summary>
    ///		Determines whether the specified <paramref name="obj"/> is an <see cref="Error"/>, and if so is equal to the current instance.
    /// </summary>
    /// <param name="obj">An <see cref="object"/> to compare with this instance.</param>
    /// <returns>
    ///		<see langword="true"/> if the specified <paramref name="obj"/> is an <see cref="Error"/>,
    ///		and is equal to this instance; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     If <paramref name="obj"/> is an <see cref="Error"/>, returns <see cref="Equals(Error)"/>.
    ///     Otherwise, returns <see langword="false"/>.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Error other && Equals(other);

    /// <summary>
    ///     Determines whether the specified <paramref name="other"/> is equal to the current instance.
    /// </summary>
    /// <param name="other">The other <see cref="Error"/> to compare with this instance.</param>
    /// <returns>
    ///		<see langword="true"/> if the specified <paramref name="other"/> is equal to this instance; otherwise, <see langword="false"/>.
    /// </returns>
    /// <summary>
    ///     Compares <see cref="Code"/> and <see cref="Message"/> for equality.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public virtual bool Equals(Error? other)
    {
        if (other is null)
            return false;

        return Code.Equals(other.Code, CodeComparisonType)
            && Message.Equals(other.Message, MessageComparisonType);
    }

    /// <summary>
    ///		Determines whether the <paramref name="left"/> is equal to the <paramref name="right"/>.
    /// </summary>
    /// <param name="left">The left-hand <see cref="Error"/>.</param>
    /// <param name="right">The right-hand <see cref="Error"/>.</param>
    /// <returns>
    ///		<see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <see cref="Equals(Error)"/> for how <see cref="Error"/> equality is calculated.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(in Error left, in Error right)
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
    /// <param name="left">The left-hand <see cref="Error"/>.</param>
    /// <param name="right">The right-hand <see cref="Error"/>.</param>
    /// <returns>
    ///		<see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <see cref="Equals(Error)"/> for how <see cref="Error"/> equality is calculated.
    ///	</remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(in Error left, in Error right) =>
        !(left == right);

    /// <summary>
    ///		Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    ///     <para>
    ///		    A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
    ///		</para>
    ///     <para>
    ///         The hash code is calculated from <see cref="Code"/>, <see cref="Message"/>.
    ///     </para>
    /// </returns>
    [Pure]
    public override int GetHashCode() =>
        HashCode.Combine(
            Code.GetHashCode(CodeComparisonType),
            Message.GetHashCode(MessageComparisonType)
        );

    /// <summary>
    ///		Returns a <see cref="string"/> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string"/> that represents this instance.</returns>
    /// <remarks>
    ///     The string contains the <see cref="Message"/>, and <see cref="Code"/> if it is not default.
    /// </remarks>
    [Pure]
    public override string ToString() =>
        ToString(includeCode: true);

    /// <summary>
    ///		Returns a <see cref="string"/> that represents this instance.
    /// </summary>
    /// <param name="includeCode">Whether to include the <see cref="Code"/>. If the code is default, it is ignored regardless of this parameter.</param>
    /// <returns>A <see cref="string"/> that represents this instance.</returns>
    /// <remarks>
    ///     The string contains the <see cref="Message"/>, and <see cref="Code"/> if it is not default and <paramref name="includeCode"/> is <see langword="true"/>.
    /// </remarks>
    [Pure]
    public string ToString(bool includeCode)
    {
        var code = Code;
        var message = Message;

        if (!includeCode || code == DefaultCode)
        {
            var shortOutputLength = 7 + message.Length;
            return string.Create(
                null,
                stackalloc char[shortOutputLength],
                $"Error({message})"
            );
        }

        var longOutputLength = 9 + code.Length + message.Length;
        return string.Create(
            null,
            stackalloc char[longOutputLength],
            $"Error({code}: {message})"
        );
    }

    /// <summary>
    ///     Creates an <see cref="Error"/> with the specified <paramref name="message"/>.
    /// </summary>
    /// <param name="message">The message to create the error with.</param>
    /// <returns>The created <see cref="Error"/>.</returns>
    [Pure]
    public static Error From(string message) =>
        new(DefaultCode, message, null);

    /// <summary>
    ///     Creates an <see cref="Error"/> with the specified <paramref name="code"/> and <paramref name="message"/>.
    /// </summary>
    /// <param name="code">The code to create the error with.</param>
    /// <param name="message">The message to create the error with.</param>
    /// <returns>The created <see cref="Error"/>.</returns>
    [Pure]
    public static Error From(string code, string message) =>
        new(code, message, null);

    /// <summary>
    ///     Creates an <see cref="Error"/> with all properties populated.
    /// </summary>
    /// <param name="code">The code to create the error with.</param>
    /// <param name="message">The message to create the error with.</param>
    /// <param name="causedBy">The exception which caused this error.</param>
    /// <returns>The created <see cref="Error"/>.</returns>
    [Pure]
    public static Error From(string code, string message, Exception? causedBy) =>
        new(code, message, causedBy);

    // Debugger proxy exposes members nicely
    [SuppressMessage("Major Code Smell", "S1144: Unused private types or members should be removed", Justification = "These members are used by the debugger.")]
    private sealed class DebuggerTypeProxy
    {
        private readonly Error _error;

        public DebuggerTypeProxy(Error error)
        {
            _error = error;
        }

        public string Code => _error.Code;
        public string Message => _error.Message;

        public Exception? CausedBy =>
            _error.CausedBy.HasValue
            ? _error.CausedBy.Value
            : null;
    }
}
