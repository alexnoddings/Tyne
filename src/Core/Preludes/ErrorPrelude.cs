using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Tyne.Preludes.Core;

/// <summary>
///     Preludes for <see cref="Tyne.Error"/>.
/// </summary>
/// <remarks>
///     See <see href="https://alexnoddings.github.io/Tyne/docs/preludes.html">preludes documentation</see>.
/// </remarks>
public static class ErrorPrelude
{
    /// <inheritdoc cref="Error.From(string)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Error Error(string message) =>
        Tyne.Error.From(message);

    /// <inheritdoc cref="Error.From(string, string)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Error Error(string code, string message) =>
        Tyne.Error.From(code, message);

    /// <inheritdoc cref="Error.From(string, string, Exception?)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Error Error(string code, string message, Exception? causedBy) =>
        Tyne.Error.From(code, message, causedBy);
}
