using MudBlazor;

namespace Tyne.Example.Client.Infrastructure;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class PageMaxWidthAttribute : Attribute
{
    public MaxWidth MaxWidth { get; }

    public PageMaxWidthAttribute(MaxWidth maxWidth)
    {
        MaxWidth = maxWidth;
    }
}
