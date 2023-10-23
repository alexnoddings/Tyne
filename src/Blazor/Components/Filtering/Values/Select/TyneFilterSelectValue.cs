using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor.Filtering.Values;

/// <inheritdoc/>
public abstract class TyneFilterSelectValue<TRequest, TValue, TSelectValue> : TyneFilterSelectValueBase<TRequest, TValue, TSelectValue>, IFilterSelectValueContext<TSelectValue>
{
    /// <summary>
    ///     A <see cref="RenderFragment"/> of <see cref="TyneFilterSelectItem{TValue}"/>s for the select-able values.
    /// </summary>
    [Parameter, EditorRequired]
    public RenderFragment Values { get; set; } = EmptyRenderFragment.Instance;

    private readonly HashSet<ItemHandle> _selectItemHandles = new();

    // Used to force LoadAvailableValuesAsync to wait for Values to have rendered
    // so that the specified values can attach themselves to this instance.
    private readonly TaskCompletionSource _waitForFirstRenderCompletion = new();

    /// <summary>
    ///     Loads available values from the values registered from <see cref="Values"/>.
    /// </summary>
    /// <inheritdoc/>
    protected override async Task<List<IFilterSelectItem<TSelectValue?>>> LoadAvailableValuesAsync()
    {
        var waitForFirstRender = _waitForFirstRenderCompletion.Task;
        if (!waitForFirstRender.IsCompletedSuccessfully)
            await waitForFirstRender.ConfigureAwait(false);

        return _selectItemHandles.Select(handle => handle.Item).ToList();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        // Complete the wait task
        if (firstRender)
            _waitForFirstRenderCompletion.SetResult();

        base.OnAfterRender(firstRender);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.OpenComponent<CascadingValue<IFilterSelectValueContext<TSelectValue>>>(0);
        builder.AddAttribute(1, nameof(CascadingValue<object>.Value), this);
        builder.AddAttribute(2, nameof(CascadingValue<object>.IsFixed), true);
        builder.AddAttribute(3, nameof(CascadingValue<object>.ChildContent), Values);
        builder.CloseComponent();
    }

    public ISelectValueItemHandle Attach(IFilterSelectItem<TSelectValue> item)
    {
        if (_selectItemHandles.Any(handle => handle.Item == item))
            throw new InvalidOperationException("Value is already attached.");

        var handle = new ItemHandle(this, item);
        _selectItemHandles.Add(handle);
        return handle;
    }

    private sealed class ItemHandle : ISelectValueItemHandle
    {
        private readonly TyneFilterSelectValue<TRequest, TValue, TSelectValue> _value;
        public IFilterSelectItem<TSelectValue?> Item { get; }

        public ItemHandle(TyneFilterSelectValue<TRequest, TValue, TSelectValue> value, IFilterSelectItem<TSelectValue> item)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
            Item = item ?? throw new ArgumentNullException(nameof(item));
        }

        public void Dispose() =>
            _value._selectItemHandles.Remove(this);
    }
}
