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

    /// <inheritdoc cref="Result.Error{T}(int, string)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T> Error<T>(int code, string message) =>
        Result.Error<T>(code, message);

    /// <inheritdoc cref="Result.Error{T}(int, string, Exception)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T> Error<T>(int code, string message, Exception causedBy) =>
        Result.Error<T>(code, message, causedBy);

    /// <inheritdoc cref="Result.Error{T}(in Error)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T> Error<T>(in Error error) =>
        Result.Error<T>(error);
}

/// <summary>
///     Preludes for <see cref="Option{T}"/>.
/// </summary>
/// <remarks>
///     See <see href="https://alexnoddings.github.io/Tyne/docs/preludes.html">preludes documentation</see>.
/// </remarks>
public static class OptionPrelude
{
    /// <inheritdoc cref="Option.None{T}()"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref readonly Option<T> None<T>() =>
        ref Option.None<T>();

    /// <inheritdoc cref="Option.Some{T}(T)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Some<T>(T value) =>
        Option.Some(value);
}

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

    /// <inheritdoc cref="Error.From(int, string)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Error Error(int code, string message) =>
        Tyne.Error.From(code, message);

    /// <inheritdoc cref="Error.From(int, string, Exception?)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Error Error(int code, string message, Exception? causedBy) =>
        Tyne.Error.From(code, message, causedBy);
}
