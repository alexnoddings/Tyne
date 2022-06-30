using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq.Expressions;

namespace Tyne.UI.Forms;

/// <summary>
///		Renders errors in a form for the <see cref="For"/> field.
/// </summary>
/// <remarks>
///		Useful for non-simple fields, such as multiselects, whose For="" only supports T, not IEnumerable<T>.
/// </remarks>
public sealed class TyneFormErrors : ComponentBase, IDisposable
{
	[CascadingParameter]
	private EditContext CurrentEditContext { get; set; } = default!;

	private EditContext? PreviousEditContext { get; set; }

	/// <summary>
	///		An <see cref="Expression"/> which accesses a field on the model.
	/// </summary>
	/// <remarks>
	///		This does not provide the model as a parameter.
	///		As this must be within an <see cref="EditContext"/>, you should have a reference to the model already.
	///		This expression is only for deriving the name of the field to access; it is never executed.
	/// </remarks>
	[Parameter]
	[EditorRequired]
	public Expression<Func<object?>> For { get; set; } = default!;
	private FieldIdentifier ForFieldIdentifier { get; set; }

	/// <summary>
	///		The content to render when there are errors for the <see cref="For"/>. This is only rendered when there are errors, otherwise it will be skipped.
	/// </summary>
	[Parameter]
	[EditorRequired]
	public RenderFragment<string[]> ChildContent { get; set; } = _ => _ => { };

	private string[] ErrorMessages { get; set; } = Array.Empty<string>();

	protected override void OnParametersSet()
	{
		if (CurrentEditContext != PreviousEditContext)
		{
			// If the edit context has been changed, then detatch the validation state changed listener from the previous edit context
			DetachValidationStateChangedListener();
			// Then hook into the validation state changed for the new context
			CurrentEditContext.OnValidationStateChanged += OnEditContextValidationStateChanged;
			// And update the old reference
			PreviousEditContext = CurrentEditContext;
		}
		// Don't bother checking if For was changed before re-creating, the parameters aren't updated often, and we don't need any special processing
		ForFieldIdentifier = FieldIdentifier.Create(For);
	}
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		if (ErrorMessages.Length > 0)
			builder.AddContent(0, ChildContent(ErrorMessages));
	}

	private void OnEditContextValidationStateChanged(object? sender, ValidationStateChangedEventArgs args)
	{
		ErrorMessages = CurrentEditContext.GetValidationMessages(ForFieldIdentifier).ToArray();
	}

	private void DetachValidationStateChangedListener()
	{
		if (PreviousEditContext != null)
			PreviousEditContext.OnValidationStateChanged -= OnEditContextValidationStateChanged;
	}

	public void Dispose()
	{
		DetachValidationStateChangedListener();
	}
}
