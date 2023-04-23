using System.Diagnostics.CodeAnalysis;

namespace Tyne.Blazor;

[SuppressMessage("Major Code Smell", "S2326: Unused type parameters should be removed", Justification = "Used for consistency across other column code.")]
public interface ITyneTable<out TRequest, TResponse> : ITyneTable
{
    public IDisposable RegisterColumn(ITyneFilteredColumn<TRequest> column);
}
