using MudBlazor;

namespace Tyne.Aerospace.Client.Infrastructure;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class PageMaxWidthAttribute : Attribute
{
    public MaxWidth MaxWidth { get; }

    public PageMaxWidthAttribute(MaxWidth maxWidth)
    {
        MaxWidth = maxWidth;
    }
}
