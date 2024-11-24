using System.Diagnostics.CodeAnalysis;

namespace Tyne.Blazor.Filtering.Context;

/// <summary>
///     Base class for <see cref="FilterValueHandle{TRequest, TValue}"/> which provides
///     a way to access <see cref="FilterInstance"/> and <see cref="Dispose"/> without
///     needing to know the generic value type the value represents.
/// </summary>
/// <typeparam name="TRequest">The type of request which loads data in the context.</typeparam>
[SuppressMessage("Major Code Smell", "S3881: \"IDisposable\" should be implemented correctly", Justification = "Implementers should implement it.")]
internal abstract class FilterValueHandle<TRequest> : IDisposable
{
    internal abstract IFilter<TRequest> FilterInstance { get; }

    public abstract void Dispose();
}
