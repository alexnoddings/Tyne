using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using MudBlazor;
using System.Reflection;

namespace Tyne.Blazor;

public partial class TyneDateRangeColumn<TRequest, TResponse> : TyneFilteredColumnBase<TRequest, TResponse>
{
    private DateRange DateRange { get; set; } = new(null, null);
    public override bool IsFilterActive =>
        DateRange.Start is not null
        || DateRange.End is not null;

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
        ColumnHelpers.UpdatePropertyInfo(ForMin, ref _previousForMin, ref _forMinPropertyInfo);
        ColumnHelpers.UpdatePropertyInfo(ForMax, ref _previousForMax, ref _forMaxPropertyInfo);
    }

    public override void ConfigureRequest(TRequest request)
    {
        if (!IsFilterActive)
            return;

        _forMinPropertyInfo?.SetValue(request, DateRange.Start);

        var end =
            MaxBehaviour is TyneDateRangeMaxBehaviour.Inclusive
            ? DateRange.End?.AddDays(1).AddMicroseconds(-1d)
            : DateRange.End;

        _forMaxPropertyInfo?.SetValue(request, end);
    }

    private async Task UpdateDateRangeAsync(DateRange newRange)
    {
        if (DateRange == newRange)
            return;

        DateRange = newRange;
        await OnUpdatedAsync().ConfigureAwait(true);
    }

    public override async Task ClearInputAsync() =>
        await UpdateDateRangeAsync(new DateRange(null, null)).ConfigureAwait(true);
}
