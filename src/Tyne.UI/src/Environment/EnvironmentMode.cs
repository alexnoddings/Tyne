namespace Tyne.UI;

/// <summary>
///		The filter mode for <see cref="Environment"/>.
/// </summary>
public enum EnvironmentMode
{
	/// <summary>
	///		The <see cref="Environment"/> will render <see cref="Environment.ChildContent"/> when the environment name is in <see cref="Environment.Filter"/>.
	/// </summary>
	Include,
	/// <summary>
	///		The <see cref="Environment"/> will render <see cref="Environment.ChildContent"/> when the environment name is <b>not</b> in <see cref="Environment.Filter"/>.
	/// </summary>
	Exclude
}
