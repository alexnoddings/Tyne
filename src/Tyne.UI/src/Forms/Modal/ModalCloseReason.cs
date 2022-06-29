namespace Tyne.UI.Forms;

/// <summary>
///		Why a modal is being closed.
/// </summary>
public enum ModalCloseReason
{
	/// <summary>
	///		The user has manually cancelled their activity.
	///		This is caused when the user explicitly signals to cancel,
	///		such as by clicking a "cancel" button, or an "X".
	/// </summary>
	Cancelled,
	/// <summary>
	///		The user has dismissed their activity.
	///		This is caused when the user implicitly signals to cancel,
	///		such as by clicking behind or to the side of a dialogue.
	/// </summary>
	Dismissed,
	/// <summary>
	///		The user has completed their activity.
	///		This is caused when the user successfully saves or completes an action,
	///		such as by clicking a "save" button, and the Modal is set to auto-close.
	/// </summary>
	Saved,
}
