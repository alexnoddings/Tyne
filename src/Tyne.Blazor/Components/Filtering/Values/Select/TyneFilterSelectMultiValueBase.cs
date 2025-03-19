namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     A <see cref="TyneFilterValue{TRequest, TValue}"/> which supports single-item selection through
///     <see cref="TyneFilterSelectValueBase{TRequest, TValue, TSelectValue}.LoadAvailableValuesAsync"/>.
/// </summary>
/// <inheritdoc/>
public abstract class TyneFilterSelectMultiValueBase<TRequest, TValue> : TyneFilterSelectValueBase<TRequest, HashSet<TValue>, TValue>
{
    // This class doesn't implement anything, it's just designed to simplify inheritance signatures
}
