using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using System.Reflection;
using Tyne.Utilities;

namespace Tyne.UI.Tables;

public partial class TyneColumn<TResult> : ComponentBase
{
	[Parameter]
	public RenderFragment? ChildContent { get; set; }

	protected PropertyInfo? OrderByProperty { get; private set; }
	protected string OrderByName { get; private set; } = string.Empty;

	private Expression<Func<TResult, object?>>? _orderBy;
	[Parameter]
	public Expression<Func<TResult, object?>>? OrderBy
	{
		get => _orderBy;
		set
		{
			_orderBy = value;
			if (value is null)
			{
				OrderByProperty = null;
				OrderByName = string.Empty;
			}
			else
			{
				OrderByProperty = ExpressionHelper.GetPropertyInfo(value);
				OrderByName = OrderByProperty.Name;
			}
		}
	}
}
