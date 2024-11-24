using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Tyne.Blazor.Localisation;

/// <summary>
///     Localises the <see cref="TyneLocalisedDateTimeBase.DateTimeUtc"/> parameter and renders it with <see cref="ChildContent"/>.
/// </summary>
/// <remarks>
///     The <see cref="LoadingContent"/> content is rendered temporarily while Tyne loads the user's time zone info.
///     See <see cref="TyneLocalisedDateTimeBase"/> for more info on this.
/// </remarks>
public class TyneLocalisedDateTime : TyneLocalisedDateTimeBase
{
    /// <summary>
    ///     Renders the localised <see cref="DateTimeOffset"/>.
    /// </summary>
    [Parameter]
    public RenderFragment<DateTimeOffset> ChildContent { get; set; } = EmptyRenderFragment.For<DateTimeOffset>();

    /// <summary>
    ///     Renders while the localised <see cref="DateTimeOffset"/> is being loaded.
    /// </summary>
    /// <remarks>
    ///     This may be <see langword="null"/>, in which case no content is rendered during loading.
    /// </remarks>
    [Parameter]
    public RenderFragment? LoadingContent { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        if (DateTimeLocal is null)
            builder.AddContent(0, LoadingContent);
        else
            builder.AddContent(1, ChildContent, DateTimeLocal.Value);
    }
}
