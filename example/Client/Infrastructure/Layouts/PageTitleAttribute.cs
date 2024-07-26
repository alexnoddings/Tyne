namespace Tyne.Aerospace.Client.Infrastructure;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class PageTitleAttribute : Attribute
{
    public string Title { get; }

    public PageTitleAttribute(string title)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
    }
}
