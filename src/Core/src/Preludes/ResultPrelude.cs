using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Tyne.Preludes.Core;

/// <summary>
///     Preludes for <see cref="Result{T}"/>.
/// </summary>
/// <remarks>
///     See <see href="https://alexnoddings.github.io/Tyne/docs/preludes.html">preludes documentation</see>.
/// </remarks>
public static class ResultPrelude
{
    /// <inheritdoc cref="Result.Ok{T}(in T)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T> Ok<T>(T value) =>
        Result.Ok(value);

    /// <inheritdoc cref="Result.Error{T}(string)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T> Error<T>(string message) =>
        Result.Error<T>(message);

    /// <inheritdoc cref="Result.Error{T}(string, string)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T> Error<T>(string code, string message) =>
        Result.Error<T>(code, message);

    /// <inheritdoc cref="Result.Error{T}(string, string, Exception)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T> Error<T>(string code, string message, Exception causedBy) =>
        Result.Error<T>(code, message, causedBy);

    /// <inheritdoc cref="Result.Error{T}(in Error)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T> Error<T>(in Error error) =>
        Result.Error<T>(error);
}
