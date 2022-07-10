namespace Tyne.Actions;

/// <summary>
///     How actions should be registered.
/// </summary>
public enum RegisterActionsMode
{
	/// <summary>
	///     All non-abstract classes implementing <see cref="IAction{TInput, TOutput}"/> are registered,
	///     unless they are decorated with <see cref="RegisterActionAttribute"/> with <see cref="RegisterActionAttribute.ShouldRegister"/> set to <see langword="false"/>.
	/// </summary>
	Implicit,
	/// <summary>
	///     Only non-abstract classes implementing <see cref="IAction{TInput, TOutput}"/> are registered,
	///     who also have a <see cref="RegisterActionAttribute"/> with <see cref="RegisterActionAttribute.ShouldRegister"/> set to <see langword="true"/>.
	/// </summary>
	Explicit
}
