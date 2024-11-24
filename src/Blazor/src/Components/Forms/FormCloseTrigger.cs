namespace Tyne.Blazor;

public enum FormCloseTrigger
{
    /// <summary>
    ///     The user has explicitly closed the form, such as by clicking a close button.
    /// </summary>
    Closed,
    /// <summary>
    ///     The user has dismissed the form, such as clicking behind it.
    /// </summary>
    Dismissed,
    /// <summary>
    ///     Some code has made a call to close the form.
    /// </summary>
    FromCode
}
