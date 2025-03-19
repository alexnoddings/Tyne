using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Tyne;

/// <summary>
///     The exception that is thrown when performing an invalid operation with a <see cref="Result{T}"/>.
/// </summary>
/// <remarks>
///     This may occur during result creation (e.g. passing a null value to <see cref="Result.Ok{T}(in T)"/>),
///     or while accessing a result (e.g. calling <see cref="Result{T}.Value"/> when it is an error).
/// </remarks>
public class BadResultException : InvalidOperationException
{
    internal static string DefaultMessage
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ExceptionMessages.Result_Invalid;
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="BadResultException"/> with a default error message.
    /// </summary>
    public BadResultException()
        : this(DefaultMessage)
    {
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="BadResultException"/> with a specified error <paramref name="message"/>.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public BadResultException(string message)
        : base(MessageOrDefault(message))
    {
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="BadResultException"/> with a default error message and a reference to the <paramref name="innerException"/> that is the cause of this exception.
    /// </summary>
    /// <param name="innerException">The <see cref="Exception"/> that is the cause of the current exception.</param>
    public BadResultException(Exception innerException)
        : this(DefaultMessage, innerException)
    {
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="BadResultException"/> with a specified error <paramref name="message"/> and a reference to the <paramref name="innerException"/> that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The <see cref="Exception"/> that is the cause of the current exception.</param>
    public BadResultException(string message, Exception innerException)
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
