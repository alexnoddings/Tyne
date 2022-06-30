namespace Tyne.UI.Forms;

/// <summary>
///     The state of a form.
/// </summary>
public enum FormState
{
	/// <summary>
	///     The form is closed.
	/// </summary>
	Closed,
	/// <summary>
	///     The form is loading data before being <see cref="Ready"/>.
	/// </summary>
	Loading,
	/// <summary>
	///     The form is ready to be used.
	/// </summary>
	Ready,
	/// <summary>
	///     The form is saving data.
	/// </summary>
	Saving
}
