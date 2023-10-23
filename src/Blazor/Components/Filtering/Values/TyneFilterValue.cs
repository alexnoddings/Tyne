using System.Linq.Expressions;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     A <see cref="IFilterValue{TRequest, TValue}"/> which is
///     configured through <c>[<see cref="ParameterAttribute"/>]</c>s.
/// </summary>
/// <inheritdoc/>
public class TyneFilterValue<TRequest, TValue> : TyneFilterValueBase<TRequest, TValue>
{
    /// <summary>
    ///     The <typeparamref name="TValue"/> property on <typeparamref name="TRequest"/>
    ///     which this value is for.
    /// </summary>
    [Parameter]
    public Expression<Func<TRequest, TValue>> For { get; set; } = null!;
    private readonly TynePropertyKeyCache<TRequest, TValue> _forCache = new(onlyUpdateOnce: true);
    protected override TyneKey ForKey => _forCache.Update(For);

    /// <summary>
    ///     The key to use when persisting <typeparamref name="TValue"/>.
    /// </summary>
    /// <remarks>
    ///     Use <see langword="null"/> to disable persistence,
    ///     or <c>"*"</c> to use the name of the property which <see cref="For"/> points to.
    /// </remarks>
    [Parameter]
    public string? PersistAs { get; set; }
    protected override TyneKey PersistKey => TyneKey.From(PersistAs, _forCache.PropertyInfo);

    /// <summary>
    ///     The default <typeparamref name="TValue"/> to use if one
    ///     is not loaded from persistence.
    /// </summary>
    [Parameter]
    public TValue? Default { get; set; }
    protected override TValue? DefaultValue => Default;

    /// <summary>
    ///     Invoked when the <typeparamref name="TValue"/> is updated.
    /// </summary>
    [Parameter]
    public EventCallback<TValue?> ValueChanged { get; set; }

    protected override Task SetValueAsync(TValue? newValue, SetValueBehaviour behaviour)
    {
        if (ValueChanged.HasDelegate)
            return SetValueAndNotifyChangedAsync(newValue, behaviour);

        return base.SetValueAsync(newValue, behaviour);
    }

    private async Task SetValueAndNotifyChangedAsync(TValue? newValue, SetValueBehaviour behaviour)
    {
        await base.SetValueAsync(newValue, behaviour).ConfigureAwait(false);
        await ValueChanged.InvokeAsync(newValue).ConfigureAwait(false);
    }
}

