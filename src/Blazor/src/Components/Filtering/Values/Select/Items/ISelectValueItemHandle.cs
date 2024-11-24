namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     A handle given to a <see cref="IFilterSelectItem{TValue}"/> when it attaches to a <see cref="IFilterSelectValueContext{TValue}"/>.
/// </summary>
/// <remarks>
///     This handle should be disposed of once the <see cref="IFilterSelectItem{TValue}"/>
///     goes out of scope to ensure it is detached from the <see cref="IFilterSelectValueContext{TValue}"/>.
/// </remarks>
public interface ISelectValueItemHandle : IDisposable
{
}
