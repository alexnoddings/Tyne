namespace Tyne.Docs.Services.Pages;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class DocsPageAttribute : Attribute
{
	public PageCategory Category { get; }
	public string Title { get; }

	public DocsPageAttribute(PageCategory category, string title)
	{
		Category = category;
		Title = title;
	}
}
