using System.Diagnostics.CodeAnalysis;
using Tyne.Blazor.Filtering.Controllers;

namespace Tyne.Blazor.Filtering.Context;

/// <summary>
///     Base class for <see cref="FilterControllerHandle{TRequest, TValue}"/>
///     which provides access to non-generic way handle functionality.
/// </summary>
[SuppressMessage("Major Code Smell", "S3881: \"IDisposable\" should be implemented correctly", Justification = "Implementers should implement it.")]
internal abstract class FilterControllerHandle : IDisposable
{
    internal abstract IFilterController FilterControllerBase { get; }

    public abstract void Dispose();
}
