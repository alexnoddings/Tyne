namespace Tyne.Aerospace.Client.Features.Systems;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class SystemTitleAttribute : Attribute
{
    public string Title { get; }

    public SystemTitleAttribute(string title)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
    }
}
