using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Tyne;

/// <summary>
///     The exception that is thrown when performing an invalid operation with an <see cref="Option{T}"/>.
/// </summary>
/// <remarks>
///     This may occur during creation (e.g. passing a null value to <see cref="Option.Some{T}(T)"/>),
///     or while accessing an option (e.g. calling <see cref="Option{T}.Value"/> when it is <c>None</c>).
/// </remarks>
public class BadOptionException : InvalidOperationException
{
    internal static string DefaultMessage
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ExceptionMessages.Option_Invalid;
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="BadOptionException"/> with a default error message.
    /// </summary>
    public BadOptionException()
        : this(DefaultMessage)
    {
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="BadOptionException"/> with a specified error <paramref name="message"/>.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public BadOptionException(string message)
        : base(MessageOrDefault(message))
    {
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="BadOptionException"/> with a default error message and a reference to the <paramref name="innerException"/> that is the cause of this exception.
    /// </summary>
    /// <param name="innerException">The <see cref="Exception"/> that is the cause of the current exception.</param>
    public BadOptionException(Exception innerException)
        : this(DefaultMessage, innerException)
    {
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="BadOptionException"/> with a specified error <paramref name="message"/> and a reference to the <paramref name="innerException"/> that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The <see cref="Exception"/> that is the cause of the current exception.</param>
    public BadOptionException(string message, Exception innerException)
        : base(MessageOrDefault(message), innerException)
    {
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string MessageOrDefault(string message) =>
        string.IsNullOrWhiteSpace(message)
        ? DefaultMessage
        : message;
}
