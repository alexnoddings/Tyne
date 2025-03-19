using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Tyne.Preludes.Core;

/// <summary>
///     Preludes for <see cref="Result{T, TE}"/>.
/// </summary>
/// <remarks>
///     See <see href="https://alexnoddings.github.io/Tyne/docs/preludes.html">preludes documentation</see>.
/// </remarks>
[SuppressMessage("Naming", "CA1715: Identifiers should have correct prefix.")]
[ExcludeFromCodeCoverage(Justification = "These methods are just convenience methods over the Result type.")]
public static class ResultPrelude
{
    /// <inheritdoc cref="Result.Ok{T, TE}(in T)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T, TE> Ok<T, TE>([DisallowNull] in T value) =>
        Result.Ok<T, TE>(value);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<Unit, TE> Ok<TE>() => Result.Cache<TE>.OkUnit;

    /// <inheritdoc cref="Result.Error{T, TE}(in TE)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T, TE> Error<T, TE>([DisallowNull] in TE error) =>
        Result.Error<T, TE>(error);
}
