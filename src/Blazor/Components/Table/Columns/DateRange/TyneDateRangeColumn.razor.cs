using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using MudBlazor;
using System.Reflection;

namespace Tyne.Blazor;

public partial class TyneDateRangeColumn<TRequest, TResponse> :
    TyneFilteredColumnBase<TRequest, TResponse>,
    ITyneTableValueFilter<DateRange>
{
    public DateRange Value { get; private set; } = new(null, null);
    public override bool IsFilterActive =>
        Value.Start is not null
        || Value.End is not null;

    [Parameter]
    public Expression<Func<TRequest, DateTime?>>? ForMin { get; set; }
    private Expression<Func<TRequest, DateTime?>>? _previousForMin;
    private PropertyInfo? _forMinPropertyInfo;

    [Parameter]
    public Expression<Func<TRequest, DateTime?>>? ForMax { get; set; }
    private Expression<Func<TRequest, DateTime?>>? _previousForMax;
    private PropertyInfo? _forMaxPropertyInfo;

    [Parameter]
    public TyneDateRangeMaxBehaviour MaxBehaviour { get; set; } = TyneDateRangeMaxBehaviour.Inclusive;

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        TyneTableFilterHelpers.UpdatePropertyInfo(ForMin, ref _previousForMin, ref _forMinPropertyInfo);
        TyneTableFilterHelpers.UpdatePropertyInfo(ForMax, ref _previousForMax, ref _forMaxPropertyInfo);
    }

    public override void ConfigureRequest(TRequest request)
    {
        if (!IsFilterActive)
            return;

        _forMinPropertyInfo?.SetValue(request, Value.Start);

        var end =
            MaxBehaviour is TyneDateRangeMaxBehaviour.Inclusive
            ? Value.End?.AddDays(1).AddMicroseconds(-1d)
            : Value.End;

        _forMaxPropertyInfo?.SetValue(request, end);
    }

    public async Task<bool> SetValueAsync(DateRange? newValue, bool isSilent, CancellationToken cancellationToken = default)
    {
        if (Value == newValue)
            return false;

        Value = newValue ?? new(null, null);

        if (!isSilent)
            await OnUpdatedAsync(cancellationToken).ConfigureAwait(true);

        return true;
    }

    public override async Task<bool> ClearValueAsync(CancellationToken cancellationToken = default) =>
        await SetValueAsync(new DateRange(null, null), false, cancellationToken).ConfigureAwait(true);
}
