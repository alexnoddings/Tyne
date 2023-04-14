using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Tyne.Blazor;

/// <summary>
///     Creates empty <see cref="RenderFragment"/>s.
/// </summary>
/// <remarks>
///     Useful for non-null placeholders.
/// </remarks>
public static class EmptyRenderFragment
{
    private static readonly RenderFragment Empty = _ => { };

    /// <summary>
    ///     Creates an empty <see cref="RenderFragment"/>.
    /// </summary>
    /// <returns>A <see cref="RenderFragment"/> which does not write to the <see cref="RenderTreeBuilder"/>.</returns>
    public static RenderFragment Create() => Empty;

    /// <summary>
    ///     Creates an empty <see cref="RenderFragment{T}"/>.
    /// </summary>
    /// <returns>A <see cref="RenderFragment{T}"/> which ignores the <typeparamref name="T"/> and does not write to the <see cref="RenderTreeBuilder"/>.</returns>
    public static RenderFragment<T> Create<T>() => _ => Empty;
}
