namespace Tyne;

public class TestHumanError : IHumanError
{
	public string HumanErrorMessage { get; }

	public TestHumanError(string message)
	{
		HumanErrorMessage = message ?? throw new ArgumentException("Message cannot be empty or whitespace.", nameof(message));
	}
}
