using Microsoft.AspNetCore.Components;
using MudBlazor.Utilities;
using System.Linq.Expressions;
using System.Reflection;

namespace Tyne.Blazor;

public partial class TyneSelectColumn<TRequest, TResponse, TValue> :
    TyneAdvancedColumnBase<TRequest, TResponse, TValue>,
    ITyneSelectColumn<TValue>,
    ITyneTableValueFilter<TValue>
{
    private readonly HashSet<TyneSelectValue<TValue?>> _registeredValues = new();
    protected IEnumerable<TyneSelectValue<TValue?>> RegisteredValues => _registeredValues.AsEnumerable();

    [Parameter, EditorRequired]
    public RenderFragment Values { get; set; } = EmptyRenderFragment.Instance;

    public override bool IsFilterActive => Value is not null;

    [Parameter]
    public string? ContentWidth { get; set; }

    protected string ContentWidthStyle =>
        new StyleBuilder()
        .AddStyle("width", ContentWidth, !string.IsNullOrWhiteSpace(ContentWidth))
        .Build();

    [Parameter]
    public TyneSelectColumnType Type { get; set; }

    [Parameter]
    public Expression<Func<TRequest, TValue?>>? For { get; set; }
    private Expression<Func<TRequest, TValue?>>? _previousFor;
    private PropertyInfo? _forPropertyInfo;

    [Parameter]
    public string? PersistAs { get; set; }
    public override TyneTableKey PersistKey =>
        TyneTableKey.From(PersistAs, _forPropertyInfo);

    [Parameter]
    public string? SyncAs { get; set; }
    public override TyneTableKey SyncKey =>
        TyneTableKey.From(SyncAs, _forPropertyInfo);

    public IDisposable RegisterValue(TyneSelectValue<TValue?> value)
    {
        if (_registeredValues.Contains(value))
            throw new ArgumentException($"{nameof(TyneSelectValue<TValue>)} is already registered.");

        _registeredValues.Add(value);
        return new DisposableAction(() => _registeredValues.Remove(value));
    }

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

    public override Task<bool> ClearValueAsync(CancellationToken cancellationToken = default) =>
        SetValueAsync(default, false, cancellationToken);
}
