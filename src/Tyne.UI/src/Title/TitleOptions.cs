namespace Tyne.UI;

public class TitleOptions
{
	public const string ConfigurationSectionName = "Title";

	public string AppName { get; set; } = string.Empty;
	public string Divider { get; set; } = string.Empty;
	public bool IsSuffix { get; set; } = true;
}
