using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using System.Reflection;

namespace Tyne.Blazor;

public partial class TyneSelectColumn<TRequest, TResponse, TValue> :
    TyneSelectColumnBase<TRequest, TResponse, TValue>,
    ITyneTableValueFilter<TValue>
{
    public TValue? Value { get; protected set; }
    public override bool IsFilterActive => Value is not null;

    [Parameter]
    public TyneSelectColumnType Type { get; set; }

    [Parameter]
    public Expression<Func<TRequest, TValue?>>? For { get; set; }
    private Expression<Func<TRequest, TValue?>>? _previousFor;
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

        _forPropertyInfo?.SetValue(request, Value);
    }

    private Task<bool> SetValueAsync(TValue? newValue) =>
        SetValueAsync(newValue, false);

    public virtual async Task<bool> SetValueAsync(TValue? newValue, bool isSilent, CancellationToken cancellationToken = default)
    {
        if (EqualityComparer<TValue>.Default.Equals(newValue, Value))
            return false;

        Value = newValue;

        if (!isSilent)
            await OnUpdatedAsync(cancellationToken).ConfigureAwait(true);

        return true;
    }

    public override Task<bool> ClearValueAsync(CancellationToken cancellationToken = default) =>
        SetValueAsync(default, false, cancellationToken);
}
