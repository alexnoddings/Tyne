namespace Tyne.Blazor;

[Flags]
public enum FormValidationEvents
{
    None = 0,
    OnFieldChanged = 1 << 0,
    OnSaveRequested = 1 << 1,

    Default = OnFieldChanged | OnSaveRequested,
}
