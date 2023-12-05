using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using Tyne.Blazor.Filtering;

namespace Tyne.Blazor.Components.Filtering.Values;

public abstract class TyneSearchFilterValue<TRequest, TSearchValue, TFilterValue> : TyneSearchFilterValueBase<TRequest, TSearchValue, TFilterValue>
{
    /// <summary>
    ///     The <typeparamref name="TFilterValue"/> property on <typeparamref name="TRequest"/>
    ///     which this value is for.
    /// </summary>
    [Parameter, EditorRequired]
    public Expression<Func<TRequest, TFilterValue>> For { get; set; } = null!;
    private readonly TynePropertyKeyCache<TRequest, TFilterValue> _forCache = new(onlyUpdateOnce: true);
    protected override TyneKey ForKey => _forCache.Update(For);

    /// <summary>
    ///     The key to use when persisting <typeparamref name="TFilterValue"/>.
    /// </summary>
    /// <remarks>
    ///     Use <see langword="null"/> to disable persistence,
    ///     or <c>"*"</c> to use the name of the property which <see cref="For"/> points to.
    /// </remarks>
    [Parameter]
    public string? PersistAs { get; set; }
    protected override TyneKey PersistKey => TyneKey.From(PersistAs, _forCache.PropertyInfo);

    /// <summary>
    ///     The default <typeparamref name="TSearchValue"/> to use if one
    ///     is not loaded from persistence.
    /// </summary>
    [Parameter]
    public TSearchValue? Default { get; set; }
    protected override TSearchValue? DefaultValue => Default;

    /// <summary>
    ///     Invoked when the <typeparamref name="TSearchValue"/> is updated.
    /// </summary>
    [Parameter]
    public EventCallback<TSearchValue?> ValueChanged { get; set; }

    protected override Task SetValueAsync(TSearchValue? newValue, SetValueBehaviour behaviour)
    {
        if (ValueChanged.HasDelegate)
            return SetValueAndNotifyChangedAsync(newValue, behaviour);

        return base.SetValueAsync(newValue, behaviour);
    }

    private async Task SetValueAndNotifyChangedAsync(TSearchValue? newValue, SetValueBehaviour behaviour)
    {
        await base.SetValueAsync(newValue, behaviour).ConfigureAwait(false);
        await ValueChanged.InvokeAsync(newValue).ConfigureAwait(false);
    }
}
