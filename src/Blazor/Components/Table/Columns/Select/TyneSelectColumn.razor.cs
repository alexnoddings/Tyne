using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using System.Reflection;

namespace Tyne.Blazor;

public partial class TyneSelectColumn<TRequest, TResponse, TValue> : TyneSelectColumnBase<TRequest, TResponse, TValue>
{
    private TValue? _selectedValue;
    public override bool IsFilterActive => _selectedValue is not null;

    [Parameter]
    public TyneSelectColumnType Type { get; set; }

    [Parameter]
    public Expression<Func<TRequest, TValue?>>? For { get; set; }
    private Expression<Func<TRequest, TValue?>>? _previousFor;
    private PropertyInfo? _forPropertyInfo;

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        ColumnHelpers.UpdatePropertyInfo(For, ref _previousFor, ref _forPropertyInfo);
    }

    public override void ConfigureRequest(TRequest request)
    {
        if (!IsFilterActive)
            return;

        _forPropertyInfo?.SetValue(request, _selectedValue);
    }

    private async Task UpdateValueAsync(TValue? newValue)
    {
        if (EqualityComparer<TValue>.Default.Equals(newValue, _selectedValue))
            return;

        _selectedValue = newValue;
        await OnUpdatedAsync().ConfigureAwait(true);
    }

    public override async Task ClearInputAsync() =>
        await UpdateValueAsync(default).ConfigureAwait(true);
}
