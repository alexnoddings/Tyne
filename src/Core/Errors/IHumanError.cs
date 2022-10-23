namespace Tyne;

/// <summary>
///		Contains a simple, human-readable error message.
/// </summary>
public interface IHumanError
{
	/// <summary>
	///		A human-readable error message.
	/// </summary>
	public string HumanErrorMessage { get; }
}
