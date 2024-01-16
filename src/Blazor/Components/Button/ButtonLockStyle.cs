namespace Tyne.Blazor;

/// <summary>
///     The style of additional content <see cref="TyneButton"/>s should render when locked.
/// </summary>
public enum ButtonLockStyle
{
    /// <summary>
    ///     Do not render any locked content.
    /// </summary>
    /// <remarks>
    ///     The button will still be locked, but no additional content is rendered.
    /// </remarks>
    None,
    /// <summary>
    ///     Render a loading bar along the bottom edge of the button.
    /// </summary>
    Bar,
    /// <summary>
    ///     Render a loading spinner at the start of the button.
    /// </summary>
    SpinnerStart,
    /// <summary>
    ///     Render a loading spinner at the end of the button.
    /// </summary>
    SpinnerEnd
}
