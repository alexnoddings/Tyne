using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using MudBlazor;
using System.Reflection;

namespace Tyne.Blazor;

public partial class TyneStringColumn<TRequest, TResponse> : TyneFilteredColumnBase<TRequest, TResponse>
{
    private MudBaseInput<string>? InputRef { get; set; }
    private string? Value { get; set; }
    public override bool IsFilterActive => !string.IsNullOrWhiteSpace(Value);

    [Parameter]
    public Expression<Func<TRequest, string?>>? For { get; set; }
    private Expression<Func<TRequest, string?>>? _previousFor;
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

        _forPropertyInfo?.SetValue(request, Value);
    }

    private async Task UpdateValueAsync(string? newValue)
    {
        if (newValue == Value)
            return;

        Value = newValue;
        await OnUpdatedAsync().ConfigureAwait(true);
    }

    public override async Task ClearInputAsync()
    {
        await UpdateValueAsync(null).ConfigureAwait(true);
        if (InputRef is not null)
            await InputRef.FocusAsync().ConfigureAwait(true);
    }
}
