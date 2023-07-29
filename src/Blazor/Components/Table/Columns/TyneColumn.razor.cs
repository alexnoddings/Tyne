using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor;

/// <summary>
///     A column for use in a <see cref="TyneTableBase{TRequest, TResponse}"/>.
/// </summary>
/// <typeparam name="TRequest">The type of request.</typeparam>
/// <typeparam name="TResponse">The type of response.</typeparam>
[SuppressMessage("Major Code Smell", "S2326: Unused type parameters should be removed", Justification = "Used for consistency across other column code.")]
public partial class TyneColumn<TRequest, TResponse> : ComponentBase
{
    /// <summary>
    ///     The content to render inside the column header.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    ///     Optionally, a property on <typeparamref name="TResponse"/> to order the results by.
    ///     If left <see langword="null"/>, ordering will not be made available for this column.
    /// </summary>
    [Parameter]
    public Expression<Func<TResponse, object?>>? OrderBy { get; set; }
    private Expression<Func<TResponse, object?>>? _previousOrderBy;

    /// <summary>
    ///     The <see cref="PropertyInfo"/> which <see cref="OrderBy"/> accesses.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         May be <see langword="null"/> if <see cref="OrderBy"/> is null, or the <see cref="PropertyInfo"/> could not be loaded.
    ///     </para>
    ///     <para>
    ///         See <see cref="ExpressionHelper.TryGetAccessedPropertyInfo{TSource, TValue}(Expression{Func{TSource, TValue}})"/>.
    ///     </para>
    /// </remarks>
    protected PropertyInfo? OrderByProperty { get; private set; }

    protected override void OnParametersSet()
    {
        UpdateCachedOrderBy();
    }

    private void UpdateCachedOrderBy()
    {
        if (OrderBy == _previousOrderBy)
            return;

        _previousOrderBy = OrderBy;
        OrderByProperty =
            OrderBy is null
            ? null
            : ExpressionHelper.TryGetAccessedPropertyInfo(OrderBy);
    }
}
