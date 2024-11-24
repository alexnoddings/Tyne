namespace Tyne.Blazor;

/// <summary>
///     Options for configuring <see cref="TynePageTitle"/>s.
/// </summary>
public sealed class TynePageTitleOptions
{
    /// <summary>
    ///     The format to use for the page title.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         If the page title is <see langword="null"/> or empty, <see cref="Empty"/> is used instead.
    ///         This follows <see cref="string.Format(string, object?[])"/>'s rules. The page title value is <c>{0}</c>.
    ///     </para>
    ///     <para>
    ///         Example:
    ///         <code>options.Format = "{0} | Some App";</code>
    ///     </para>
    ///     <para>
    ///         If this is not set, the page title value will be used on it's own.
    ///     </para>
    /// </remarks>
    public string Format { get; set; } = string.Empty;

    /// <summary>
    ///     The page title to use when the supplied value is <see langword="null"/> or empty.
    /// </summary>
    /// <remarks>
    ///     This value is not formatted.
    /// </remarks>
    public string Empty { get; set; } = string.Empty;
}
