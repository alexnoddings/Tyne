namespace Tyne.UI.Forms;

/// <summary>
///     Extensions for working with <see cref="TyneModalForm{TOpen, TModel}"/>s.
/// </summary>
public static class ModalExtensions
{
	/// <summary>
	///     Opens a form whose open input is <see cref="Unit"/>.
	/// </summary>
	/// <typeparam name="TModel">The form's model.</typeparam>
	/// <param name="form">The form.</param>
	/// <returns>A <see cref="Task"/> representing the call to <see cref="TyneModalForm{TOpen, TModel}.OpenAsync(TOpen)"/>.</returns>
	/// <remarks>
	///     This is a helper function to make binding buttons to opening modal forms easier in Blazor.
	///     It cuts OnClick="() => SomeForm.OpenAsync(Unit.Value)" down to OnClick="SomeForm.OpenAsync".
	/// </remarks>
	public static Task OpenAsync<TModel>(this TyneModalForm<Unit, TModel> form) where TModel : class =>
		form.OpenAsync(Unit.Value);
}
