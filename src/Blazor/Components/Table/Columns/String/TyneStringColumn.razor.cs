using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using MudBlazor;
using System.Reflection;

namespace Tyne.Blazor;

public partial class TyneStringColumn<TRequest, TResponse> :
    TynePersistedColumnBase<TRequest, TResponse, string>
{
    private MudBaseInput<string>? InputRef { get; set; }
    public override bool IsFilterActive => !string.IsNullOrWhiteSpace(Value);

    [Parameter]
    public Expression<Func<TRequest, string?>>? For { get; set; }
    private Expression<Func<TRequest, string?>>? _previousFor;
    private PropertyInfo? _forPropertyInfo;

    [Parameter]
    public string? PersistAs { get; set; }
    public override TyneFilterPersistKey PersistKey =>
        TyneFilterPersistKey.From(PersistAs, _forPropertyInfo);

    protected override async Task OnInitializedAsync()
    {
        // Needs to come before initialisation so that PersistKey will pick up the property info
        UpdateForPropertyInfo();
        await base.OnInitializedAsync().ConfigureAwait(true);
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        UpdateForPropertyInfo();
    }

    private void UpdateForPropertyInfo() =>
        TyneTableFilterHelpers.UpdatePropertyInfo(For, ref _previousFor, ref _forPropertyInfo);

    public override void ConfigureRequest(TRequest request)
    {
        if (!IsFilterActive)
            return;

        _forPropertyInfo?.SetValue(request, Value);
    }

    public override async Task ClearValueAsync(CancellationToken cancellationToken = default)
    {
        await SetValueAsync("", false, cancellationToken).ConfigureAwait(true);
        if (InputRef is not null)
            await InputRef.FocusAsync().ConfigureAwait(true);
    }
}
