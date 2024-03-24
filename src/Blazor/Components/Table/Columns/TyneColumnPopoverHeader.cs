using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Tyne.Blazor.Tables.Columns;

/// <inheritdoc/>
public partial class TyneColumnPopoverHeader<TResponse> : TyneColumnPopoverHeaderBase<TResponse>
{
    /// <summary>
    ///     The icon to use for the popover <see cref="TyneColumnPopoverHeaderBase{TResponse}.Content"/> control.
    /// </summary>
    /// <remarks>
    ///     Leave this as <see cref="string.Empty"/> (or do not supply a parameter)
    ///     to use the default icons which change based on
    ///     <see cref="TyneColumnPopoverHeaderBase{TResponse}.IsActive"/>
    ///     and <see cref="TyneColumnPopoverHeaderBase{TResponse}.IsOpen"/>.
    /// </remarks>
    [Parameter]
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    ///     The icon to use for the popover <see cref="TyneColumnPopoverHeaderBase{TResponse}.Content"/> control.
    /// </summary>
    /// <remarks>
    ///     Uses the <see cref="Icon"/> if set,
    ///     otherwise uses defaults based on 
    ///     <see cref="TyneColumnPopoverHeaderBase{TResponse}.IsActive"/>
    ///     and <see cref="TyneColumnPopoverHeaderBase{TResponse}.IsOpen"/>.
    /// </remarks>
    protected override string FilterIcon
    {
        get
        {
            if (!string.IsNullOrEmpty(Icon))
                return Icon;

            if (IsActive)
                return Icons.Material.Filled.SavedSearch;

            if (IsOpen)
                return Icons.Material.Filled.SearchOff;

            return Icons.Material.Filled.Search;
        }
    }

    /// <summary>
    ///     The colour of the <see cref="Icon"/>.
    /// </summary>
    /// <remarks>
    ///     Leave as <see langword="null"/> (or do not supply a parameter)
    ///     to use the default colours which change based on
    ///     <see cref="TyneColumnPopoverHeaderBase{TResponse}.IsActive"/>
    ///     and <see cref="TyneColumnPopoverHeaderBase{TResponse}.IsOpen"/>.
    /// </remarks>
    [Parameter]
    public Color? IconColour { get; set; }

    /// <summary>
    ///     The colour to use for the <see cref="Icon"/>.
    /// </summary>
    /// <remarks>
    ///     Uses the <see cref="IconColour"/> if set,
    ///     otherwise uses defaults based on 
    ///     <see cref="TyneColumnPopoverHeaderBase{TResponse}.IsActive"/>
    ///     and <see cref="TyneColumnPopoverHeaderBase{TResponse}.IsOpen"/>.
    /// </remarks>
    protected override Color FilterIconColour
    {
        get
        {
            if (IconColour is not null)
                return IconColour.Value;

            if (IsActive)
                return Color.Primary;

            return Color.Default;
        }
    }
}
