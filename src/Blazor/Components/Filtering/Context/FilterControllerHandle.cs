using System.Diagnostics.CodeAnalysis;

namespace Tyne.Blazor.Filtering.Context;

/// <summary>
///     Base class for <see cref="FilterControllerHandle{TRequest, TValue}"/>
///     which provides a non-generic way to <see cref="Dispose"/> handles.
/// </summary>
[SuppressMessage("Major Code Smell", "S3881: \"IDisposable\" should be implemented correctly", Justification = "Implementers should implement it.")]
internal abstract class FilterControllerHandle : IDisposable
{
    public abstract void Dispose();
}
