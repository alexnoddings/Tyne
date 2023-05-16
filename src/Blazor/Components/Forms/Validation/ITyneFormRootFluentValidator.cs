namespace Tyne.Blazor;

public interface ITyneFormRootFluentValidator
{
    public IDisposable RegisterNestedValidator(ITyneFormFluentValidator nestedValidator);
}
