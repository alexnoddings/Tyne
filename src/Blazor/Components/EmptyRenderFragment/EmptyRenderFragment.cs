using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Tyne.Blazor;

/// <summary>
///     An empty <see cref="RenderFragment"/> which does not write to the <see cref="RenderTreeBuilder"/>.
/// </summary>
/// <remarks>
///     Useful for non-null placeholders.
/// </remarks>
public static class EmptyRenderFragment
{
    /// <summary>
    ///     An empty <see cref="RenderFragment"/> which does not write to the <see cref="RenderTreeBuilder"/>.
    /// </summary>
    public static RenderFragment Instance { get; } = _ => { };

    /// <summary>
    ///     An empty <see cref="RenderFragment{T}"/> which ignores the <typeparamref name="T"/> and does not write to the <see cref="RenderTreeBuilder"/>.
    /// </summary>
    /// <returns>An empty <see cref="RenderFragment{T}"/> which ignores the <typeparamref name="T"/> and does not write to the <see cref="RenderTreeBuilder"/>.</returns>
    public static RenderFragment<T> For<T>() => EmptyRenderFragmentT<T>.InstanceT;

    private static class EmptyRenderFragmentT<T>
    {
        /// <summary>
        ///     An empty <see cref="RenderFragment{T}"/> which ignores the <typeparamref name="T"/> and does not write to the <see cref="RenderTreeBuilder"/>.
        /// </summary>
        internal static readonly RenderFragment<T> InstanceT = _ => EmptyRenderFragment.Instance;
    }
}
