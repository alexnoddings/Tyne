using Microsoft.AspNetCore.Components;
using MudBlazor.Utilities;
using System.Linq.Expressions;
using System.Reflection;

namespace Tyne.Blazor;

public partial class TyneMultiSelectColumn<TRequest, TResponse, TValue> :
    TyneAdvancedColumnBase<TRequest, TResponse, HashSet<TyneSelectValue<TValue?>>>,
    ITyneSelectColumn<TValue>,
    ITyneTableValueFilter<HashSet<TyneSelectValue<TValue?>>>
{
    public override bool IsFilterActive => Value?.Count > 0;
    protected HashSet<TyneSelectValue<TValue?>> SelectedValues => Value ?? new();

    private readonly HashSet<TyneSelectValue<TValue?>> _registeredValues = new();
    protected IEnumerable<TyneSelectValue<TValue?>> RegisteredValues => _registeredValues.AsEnumerable();

    [Parameter, EditorRequired]
    public RenderFragment Values { get; set; } = EmptyRenderFragment.Instance;

    [Parameter]
    public string? ContentWidth { get; set; }

    protected string ContentWidthStyle =>
        new StyleBuilder()
        .AddStyle("width", ContentWidth, !string.IsNullOrWhiteSpace(ContentWidth))
        .Build();

    [Parameter]
    public TyneMultiSelectColumnType Type { get; set; }

    [Parameter]
    public Expression<Func<TRequest, List<TValue?>?>>? For { get; set; }
    private Expression<Func<TRequest, List<TValue?>?>>? _previousFor;
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

        _forPropertyInfo?.SetValue(request, SelectedValues.Select(selectValue => selectValue.Value).ToList());
    }

    private async Task UpdateSelectedAsync(TyneSelectValue<TValue?> value, bool isSelected)
    {
        var newSelectedValues = SelectedValues.ToHashSet();
        if (isSelected)
            newSelectedValues.Add(value);
        else
            newSelectedValues.Remove(value);

        await SetValueAsync(newSelectedValues, false).ConfigureAwait(true);
    }

    private async Task UpdateSelectedValuesAsync(IEnumerable<TyneSelectValue<TValue?>> newValues) =>
        await SetValueAsync(newValues.ToHashSet(), false).ConfigureAwait(true);

    public override async Task<bool> SetValueAsync(HashSet<TyneSelectValue<TValue?>>? newValue, bool isSilent, CancellationToken cancellationToken = default)
    {
        newValue ??= new();
        if (newValue.SequenceEqual(Value ?? new()))
            return false;

        return await base.SetValueAsync(newValue, isSilent, cancellationToken).ConfigureAwait(true);
    }

    public override Task<bool> ClearValueAsync(CancellationToken cancellationToken = default) =>
        SetValueAsync(new(), false, cancellationToken);

    protected override async Task<bool> InitialisePersistedValueAsync()
    {
        Task<bool> SetValueProxy(HashSet<TValue?>? values, bool isSilent, CancellationToken cancellationToken = default)
        {
#pragma warning disable BL0005
            var selectedValues =
                values is not null
                ? values.Select(value => new TyneSelectValue<TValue?> { Value = value }).ToHashSet()
                : null;
#pragma warning restore BL0005

            return SetValueAsync(selectedValues, isSilent, cancellationToken);
        }

        return await TyneTablePersistedFilterHelpers.InitialisePersistedValueAsync<HashSet<TValue?>>(PersistKey, SetValueProxy, NavigationManager, CancellationToken.None).ConfigureAwait(true);
    }

    protected override async Task UpdatePersistedValueAsync()
    {
        var selectedValues =
            SelectedValues.Count > 0
            ? SelectedValues.Select(value => value.Value)
            : null;
        await TyneTablePersistedFilterHelpers.PersistValueAsync(PersistKey, selectedValues, NavigationManager).ConfigureAwait(true);
    }
}
