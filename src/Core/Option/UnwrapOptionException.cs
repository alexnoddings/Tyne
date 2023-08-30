using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Tyne;

/// <summary>
///     The exception that is thrown when unsuccessfully unwrapping a <c>None</c> <see cref="Option{T}"/>.
/// </summary>
/// <remarks>
///     See <see cref="OptionExtensions.Unwrap{T}(in Option{T})"/> for how unwrapping works.
/// </remarks>
[Serializable]
public class UnwrapOptionException : BadOptionException
{
    internal new static string DefaultMessage
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ExceptionMessages.Option_CannotUnwrapNone;
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="UnwrapOptionException"/> with a default error message.
    /// </summary>
    public UnwrapOptionException()
        : this(DefaultMessage)
    {
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="UnwrapOptionException"/> with a specified error <paramref name="message"/>.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public UnwrapOptionException(string message)
        : base(MessageOrDefault(message))
    {
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="UnwrapOptionException"/> with a default error message and a reference to the <paramref name="innerException"/> that is the cause of this exception.
    /// </summary>
    /// <param name="innerException">The <see cref="Exception"/> that is the cause of the current exception.</param>
    public UnwrapOptionException(Exception innerException)
        : this(DefaultMessage, innerException)
    {
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="UnwrapOptionException"/> with a specified error <paramref name="message"/> and a reference to the <paramref name="innerException"/> that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The <see cref="Exception"/> that is the cause of the current exception.</param>
    public UnwrapOptionException(string message, Exception innerException)
        : base(MessageOrDefault(message), innerException)
    {
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="UnwrapOptionException"/> with serialized data.
    /// </summary>
    /// <param name="info">The object that holds the serialized object data.</param>
    /// <param name="context">The contextual information about the source or destination.</param>
    protected UnwrapOptionException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string MessageOrDefault(string message) =>
        string.IsNullOrWhiteSpace(message)
        ? DefaultMessage
        : message;
}
