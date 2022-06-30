namespace Tyne.UI.Forms;

/// <summary>
///     How quickly a modal should animate.
/// </summary>
public enum ModalAnimationSpeed
{
	/// <summary>
	///     The animation should be skipped.
	/// </summary>
	Skipped, // 0ms
	/// <summary>
	///     The animation should be faster than normal.
	/// </summary>
	Fast, // 120ms
	/// <summary>
	///     The animation should be normal speed.
	/// </summary>
	Normal, // 240ms
	/// <summary>
	///     The animation should be slower than normal.
	/// </summary>
	Slow // 400ms
}
