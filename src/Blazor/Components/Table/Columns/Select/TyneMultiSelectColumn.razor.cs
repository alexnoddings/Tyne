using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using System.Reflection;

namespace Tyne.Blazor;

public partial class TyneMultiSelectColumn<TRequest, TResponse, TValue> :
    TyneSelectColumnBase<TRequest, TResponse, TValue>,
    ITyneTableValueFilter<HashSet<TyneSelectValue<TValue?>>>
{
    private HashSet<TyneSelectValue<TValue?>> _selectedValues = new();
    public override bool IsFilterActive => _selectedValues.Count > 0;
    public HashSet<TyneSelectValue<TValue?>>? Value => _selectedValues;

    [Parameter]
    public TyneMultiSelectColumnType Type { get; set; }

    [Parameter]
    public Expression<Func<TRequest, List<TValue?>?>>? For { get; set; }
    private Expression<Func<TRequest, List<TValue?>?>>? _previousFor;
    private PropertyInfo? _forPropertyInfo;

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        TyneTableFilterHelpers.UpdatePropertyInfo(For, ref _previousFor, ref _forPropertyInfo);
    }

    public override void ConfigureRequest(TRequest request)
    {
        if (!IsFilterActive)
            return;

        _forPropertyInfo?.SetValue(request, _selectedValues.Select(selectValue => selectValue.Value).ToList());
    }

    private async Task UpdateSelectedAsync(TyneSelectValue<TValue?> value, bool isSelected)
    {
        if (isSelected)
            _selectedValues.Add(value);
        else
            _selectedValues.Remove(value);

        await OnUpdatedAsync().ConfigureAwait(true);
    }

    private async Task UpdateSelectedValuesAsync(IEnumerable<TyneSelectValue<TValue?>> newValues) =>
        await SetValueAsync(newValues.ToHashSet(), false).ConfigureAwait(true);

    public async Task<bool> SetValueAsync(HashSet<TyneSelectValue<TValue?>>? newValue, bool isSilent, CancellationToken cancellationToken = default)
    {
        newValue ??= new();
        if (_selectedValues.SequenceEqual(newValue))
            return false;

        _selectedValues = newValue;

        if (!isSilent)
            await OnUpdatedAsync(cancellationToken).ConfigureAwait(true);

        return true;
    }

    public override Task<bool> ClearValueAsync(CancellationToken cancellationToken = default) =>
        SetValueAsync(new(), false, cancellationToken);
}
