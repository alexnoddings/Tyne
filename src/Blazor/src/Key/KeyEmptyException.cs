using System.Runtime.CompilerServices;

namespace Tyne.Blazor;

/// <summary>
///     The exception that is thrown when a <see cref="TyneKey"/> is
///     expected to have a value but is <see cref="TyneKey.IsEmpty"/>.
/// </summary>
public class KeyEmptyException : Exception
{
    private const string DefaultMessage = "Key cannot be empty here.";

    /// <summary>
    ///     Initializes a new instance of <see cref="KeyEmptyException"/> with a default error message.
    /// </summary>
    public KeyEmptyException() : base(DefaultMessage)
    {
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="KeyEmptyException"/> with a specified error <paramref name="message"/>.
    /// </summary>
    /// <param name="message">An error message that explains the reason for the exception.</param>
    public KeyEmptyException(string? message) : base(message)
    {
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="KeyEmptyException"/> with a specified error <paramref name="message"/>
    ///     and a reference to the <paramref name="innerException"/> that is the cause of this exception.
    /// </summary>
    /// <param name="message">An error message that explains the reason for the exception.</param>
    /// <param name="innerException">The <see cref="Exception"/> that is the cause of the current exception.</param>
    public KeyEmptyException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    /// <summary>
    ///     Throws a <see cref="KeyEmptyException"/> with a default message if <paramref name="key"/> is empty.
    /// </summary>
    /// <param name="key">The <see cref="TyneKey"/>.</param>
    /// <param name="paramName">
    ///     The name of the parameter with which <paramref name="key"/> corresponds.
    /// </param>
    /// <exception cref="KeyEmptyException">When <paramref name="key"/> is empty.</exception>
    /// <remarks>
    ///     The <paramref name="paramName"/> parameter is included to support <see cref="CallerArgumentExpressionAttribute"/>.
    ///     It's recommended that you don't pass a value for this parameter and let the name of <paramref name="key"/> be used instead.
    /// </remarks>
    public static void ThrowIfEmpty(TyneKey key, [CallerArgumentExpression(nameof(key))] string? paramName = null) =>
        ThrowIfEmpty(key, DefaultMessage, paramName);

    /// <summary>
    ///     Throws a <see cref="KeyEmptyException"/> with <paramref name="exceptionMessage"/> if <paramref name="key"/> is empty.
    /// </summary>
    /// <param name="key">The <see cref="TyneKey"/>.</param>
    /// <param name="exceptionMessage">
    ///     A message to create the <see cref="KeyEmptyException"/> with.
    ///     If not null or empty, it will have the <paramref name="paramName"/> appended.
    /// </param>
    /// <param name="paramName">
    ///     The name of the parameter with which <paramref name="key"/> corresponds.
    /// </param>
    /// <exception cref="KeyEmptyException">When <paramref name="key"/> is empty.</exception>
    /// <remarks>
    ///     The <paramref name="paramName"/> parameter is included to support <see cref="CallerArgumentExpressionAttribute"/>.
    ///     It's recommended that you don't pass a value for this parameter and let the name of <paramref name="key"/> be used instead.
    /// </remarks>
    public static void ThrowIfEmpty(TyneKey key, string exceptionMessage, [CallerArgumentExpression(nameof(key))] string? paramName = null)
    {
        if (key.IsEmpty)
        {
            if (!string.IsNullOrEmpty(paramName))
                exceptionMessage += $" (Parameter '{paramName}')";

            throw new KeyEmptyException(exceptionMessage);
        }
    }
}
