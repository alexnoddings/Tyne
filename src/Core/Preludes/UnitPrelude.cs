using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Tyne.Preludes.Core;

/// <summary>
///     Preludes for <see cref="Unit"/>.
/// </summary>
/// <remarks>
///     See <see href="https://alexnoddings.github.io/Tyne/docs/preludes.html">preludes documentation</see>.
/// </remarks>
public static class UnitPrelude
{
    /// <inheritdoc cref="Unit.Value"/>
    [Pure]
    public static ref readonly Unit unit
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref Unit.Value;
    }
}
