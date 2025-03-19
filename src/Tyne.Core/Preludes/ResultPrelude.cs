using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Tyne.Preludes.Core;

/// <summary>
///     Preludes for <see cref="Result{T, E}"/>.
/// </summary>
/// <remarks>
///     See <see href="https://alexnoddings.github.io/Tyne/docs/preludes.html">preludes documentation</see>.
/// </remarks>
[SuppressMessage("Naming", "CA1715: Identifiers should have correct prefix.")]
[ExcludeFromCodeCoverage(Justification = "These methods are just convenience methods over the Result type.")]
public static class ResultPrelude
{
    /// <inheritdoc cref="Result.Ok{T, E}(in T)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T, E> Ok<T, E>([DisallowNull] in T value) =>
        Result.Ok<T, E>(value);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<Unit, E> Ok<E>() => Result.Cache<E>.OkUnit;

    /// <inheritdoc cref="Result.Error{T, E}(in E)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T, E> Error<T, E>([DisallowNull] in E error) =>
        Result.Error<T, E>(error);
}
