using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Tyne.Internal;

/// <summary>
///     Intended for internal use by Tyne packages working with <see cref="Tyne.Error"/>s.
/// </summary>
public static class Error
{
    internal const string DefaultCode = "default";
    internal const string DefaultMessage = "Unknown error.";

    /// <summary>
    ///     Checks whether <paramref name="code"/> is a valid <see cref="Tyne.Error.Code"/>.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <returns><see langword="true"/> if the code is a valid error code; otherwise, <see langword="false"/>.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValidCode([NotNullWhen(true)] string? code) =>
        !string.IsNullOrWhiteSpace(code);

    /// <summary>
    ///     Returns <paramref name="code"/> if it is a valid <see cref="Tyne.Error.Code"/>; otherwise, returns a default code.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <returns>A valid error code.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string CodeOrDefault(string? code) =>
        IsValidCode(code) ? code : DefaultCode;

    /// <summary>
    ///     Checks whether <paramref name="message"/> is a valid <see cref="Tyne.Error.Message"/>.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns><see langword="true"/> if the message is a valid error message; otherwise, <see langword="false"/>.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValidMessage([NotNullWhen(true)] string? message) =>
        !string.IsNullOrWhiteSpace(message);

    /// <summary>
    ///     Returns <paramref name="message"/> if it is a valid <see cref="Tyne.Error.Message"/>; otherwise, returns a default message.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>A valid error message.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string MessageOrDefault(string? message) =>
        IsValidMessage(message) ? message : DefaultMessage;
}
