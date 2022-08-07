using Microsoft.AspNetCore.Components;

namespace Tyne.UI.Tables;

public sealed class TyneSelectValue<TValue> : ComponentBase, IDisposable
{
	[CascadingParameter]
	private ITyneSelectColumn<TValue> SelectColumn { get; set; } = default!;

	[Parameter, EditorRequired]
	public TValue? Value { get; set; }

	[Parameter]
	public RenderFragment? ChildContent { get; set; }

	private IDisposable? ValueRegistration { get; set; }

	internal object AsKey => (object?)Value ?? "(null)";

	protected override void OnInitialized()
	{
		if (SelectColumn is null)
			throw new ArgumentNullException(nameof(SelectColumn), "Cascading column parameter missing.");

		ValueRegistration = SelectColumn.RegisterSelectValue(this);
	}

	public void Dispose()
	{
		if (ValueRegistration is not null)
		{
			ValueRegistration.Dispose();
			ValueRegistration = null;
		}
	}
}
