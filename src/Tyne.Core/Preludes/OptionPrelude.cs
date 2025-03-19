using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Tyne.Preludes.Core;

/// <summary>
///     Preludes for <see cref="Option{T}"/>.
/// </summary>
/// <remarks>
///     See <see href="https://alexnoddings.github.io/Tyne/docs/preludes.html">preludes documentation</see>.
/// </remarks>
[ExcludeFromCodeCoverage(Justification = "These methods are just convenience methods over the Option type.")]
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
    public static Option<T> Some<T>([DisallowNull] in T value) =>
        Option.Some(value);
}
