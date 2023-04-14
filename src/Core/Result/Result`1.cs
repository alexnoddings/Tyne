using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Tyne;

/// <summary>
///     Represents the result of performing an operation.
/// </summary>
/// <remarks>
///     <para>
///         A success result is one which does not contain any <see cref="ResultMessage"/>s of type <see cref="ResultMessageType.Error"/>.
///     </para>
///     <para>
///         Conversely, an error result is one which contains at least one <see cref="ResultMessage"/>s of type <see cref="ResultMessageType.Error"/>.
///         An error result may also not contain any <see cref="ResultMessage"/>s of type <see cref="ResultMessageType.Success"/>.
///     </para>
///     <para>
///         Both success and failure results may contain 0 or more <see cref="ResultMessage"/>s of type <see cref="ResultMessageType.Info"/> and <see cref="ResultMessageType.Warning"/>.
///     </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
[DebuggerTypeProxy(typeof(Result<>.ResultDebuggerTypeProxy))]
[JsonConverter(typeof(ResultJsonConverterFactory))]
public sealed class Result<T> : IResult, IEquatable<Result<T>>
{
    [MemberNotNullWhen(true, nameof(_value))]
    public bool WasSuccess { get; }
    public ReadOnlyCollection<ResultMessage> Messages { get; }

    private readonly T? _value;
    public T Value
    {
        get
        {
            if (!WasSuccess)
                throw new InvalidOperationException("Result was not successful.");

            if (_value is null)
                throw new InvalidOperationException("No value associated with result.");

            return _value;
        }
    }

    private Result(T? value, ReadOnlyCollection<ResultMessage> messages)
    {
        WasSuccess = Result.WasSuccess(messages);
        Messages = messages;

        if (WasSuccess && value is null)
            throw new ArgumentException("Cannot be null when the result is successful.", nameof(value));

        _value = value;
    }

    internal static Result<T> Success(T value, IList<ResultMessage> messages)
    {
        if (messages.Any(message => message.Type is ResultMessageType.Error))
            throw new InvalidOperationException($"A success result should not contain any messages of type {nameof(ResultMessageType)}.{nameof(ResultMessageType.Error)}.");

        var messagesCollection = new ReadOnlyCollection<ResultMessage>(messages);
        return new(value, messagesCollection);
    }

    internal static Result<T> Failure(IList<ResultMessage> messages)
    {
        if (messages.Any(message => message.Type is ResultMessageType.Success))
            throw new InvalidOperationException($"A failure result should not contain any messages of type {nameof(ResultMessageType)}.{nameof(ResultMessageType.Success)}.");

        if (!messages.Any(message => message.Type is ResultMessageType.Error))
            throw new InvalidOperationException($"A failure result should contain at least one message of type {nameof(ResultMessageType)}.{nameof(ResultMessageType.Error)}.");

        var messagesCollection = new ReadOnlyCollection<ResultMessage>(messages);
        return new(default, messagesCollection);
    }

    /// <summary>
    ///		Determines whether the specified <paramref name="other"/> is equal to the current instance of the same type.
    /// </summary>
    /// <param name="other">The other <see cref="Result{T}"/> to compare with this instance.</param>
    /// <returns>
    ///		<see langword="true"/> if the specified <paramref name="other"/> is equal to this instance; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///		This relies on the <see cref="Value"/> overriding <see cref="object.Equals(object?)"/>.
    ///		If it does not, this may not behave as expected.
    /// </remarks>
    public bool Equals(Result<T>? other)
    {
        if (other is null) return false;

        if (WasSuccess && other.WasSuccess)
            return _value.Equals(other._value) && Messages.SequenceEqual(other.Messages);

        if (!WasSuccess && !other.WasSuccess)
            return Messages.SequenceEqual(other.Messages);

        return false;
    }

    /// <summary>
    ///		Determines whether the specified <paramref name="obj"/> is a <see cref="Result{T}"/>, and is equal to the current instance.
    /// </summary>
    /// <param name="obj">An <see cref="object"/> to compare with this instance.</param>
    /// <returns>
    ///		<see langword="true"/> if the specified <paramref name="obj"/> is a <see cref="Result{T}"/>, and is equal to this instance; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///		This relies on the <see cref="Value"/> or <see cref="Error"/> overriding <see cref="object.Equals(object?)"/>.
    ///		If they do not, this may not behave as expected.
    /// </remarks>
    public override bool Equals(object? obj) =>
        obj is Result<T> other && Equals(other);

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
    public static bool operator ==(Result<T> left, Result<T> right)
    {
        if (left is null)
            return right is null;

        return left.Equals(right);
    }

    /// <summary>
    ///		Determines whether the <paramref name="left"/> is not equal to the <paramref name="right"/>.
    /// </summary>
    /// <param name="left">The left-hand <see cref="Result{T}"/>.</param>
    /// <param name="right">The right-hand <see cref="Result{T}"/>.</param>
    /// <returns>
    ///		<see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///		See <see cref="Equals(Result{T}?)"/>.
    ///	</remarks>
    public static bool operator !=(Result<T> left, Result<T> right)
    {
        if (left is null)
            return right is not null;

        return !left.Equals(right);
    }

    /// <summary>
    ///		Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    ///		A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
    /// </returns>
    public override int GetHashCode() =>
        HashCode.Combine(WasSuccess, _value, Messages);

    /// <summary>
    ///		Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString() =>
        WasSuccess
        ? $"Success<{typeof(T).Name}>({_value})"
        // We could render Messages of type Error here, but that could lead to an awfully long string
        : $"Error<{typeof(T).Name}>({Messages.Count(message => message.Type is ResultMessageType.Error)} errors)";

    /// <summary>
    ///     A proxy type to nicely rendering <see cref="Result{T}"/>s while debugging.
    /// </summary>
    /// <remarks>
    ///     Provides access to the nullable underlying fields rather than using the exception-based properties.
    /// </remarks>
    [SuppressMessage("Major Code Smell", "S1144: Unused private types or members should be removed", Justification = "These members are reflected by the debugger.")]
    private sealed class ResultDebuggerTypeProxy
    {
        private readonly Result<T> _result;

        public ResultDebuggerTypeProxy(Result<T> result)
        {
            _result = result;
        }

        public bool WasSuccess => _result.WasSuccess;
        public ReadOnlyCollection<ResultMessage> Messages => _result.Messages;
        public T? Value => _result.WasSuccess ? _result.Value : default;
    }
}
