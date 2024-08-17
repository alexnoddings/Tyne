using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Tyne;

/// <summary>
///     The exception that is thrown when unsuccessfully unwrapping a <see cref="Result{T}"/>.
/// </summary>
/// <remarks>
///     See <see cref="ResultExtensions.Unwrap{T}(Result{T})"/> for how unwrapping works.
/// </remarks>
public class UnwrapResultValueException : BadResultException
{
    internal static new string DefaultMessage
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ExceptionMessages.Result_CannotUnwrapValueFromError;
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="UnwrapResultValueException"/> with a default error message.
    /// </summary>
    public UnwrapResultValueException()
        : this(DefaultMessage)
    {
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="UnwrapResultValueException"/> with a specified error <paramref name="message"/>.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public UnwrapResultValueException(string message)
        : base(MessageOrDefault(message))
    {
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="UnwrapResultValueException"/> with a default error message and a reference to the <paramref name="innerException"/> that is the cause of this exception.
    /// </summary>
    /// <param name="innerException">The <see cref="Exception"/> that is the cause of the current exception.</param>
    public UnwrapResultValueException(Exception innerException)
        : this(DefaultMessage, innerException)
    {
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="UnwrapResultValueException"/> with a specified error <paramref name="message"/> and a reference to the <paramref name="innerException"/> that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The <see cref="Exception"/> that is the cause of the current exception.</param>
    public UnwrapResultValueException(string message, Exception innerException)
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
