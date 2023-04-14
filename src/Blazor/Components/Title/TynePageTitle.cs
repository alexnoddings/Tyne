using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Options;

namespace Tyne.Blazor;

/// <summary>
///		Sets the title of a page according to <see cref="TynePageTitleOptions"/> and <see cref="Value"/>.
/// </summary>
/// <remarks>
///		<para>
///			The <see cref="TynePageTitleOptions"/> can be registered using 
///			<see cref="TyneBuilderBlazorExtensions.ConfigurePageTitle(TyneBuilder)"/>
///			or <see cref="TyneBuilderBlazorExtensions.ConfigurePageTitle(TyneBuilder, Action{TynePageTitleOptions})"/>.
///		</para>
///		<para>
///			This wraps <see cref="PageTitle"/>.
///		</para>
/// </remarks>
public class TynePageTitle : ComponentBase
{
    /// <summary>
    ///		The value for the page title.
    /// </summary>
    /// <remarks>
    ///		When <see langword="null"/> or empty, <see cref="TynePageTitleOptions.Empty"/> is used (even if it is null or empty).
    ///		If <see cref="TynePageTitleOptions.Format"/> is <see langword="null"/> or empty, this value is used.
    ///		Otherwise, <see cref="TynePageTitleOptions.Format"/> is formatted with this value.
    /// </remarks>
    [Parameter]
    public string? Value { get; set; }

    [Inject]
    private IOptions<TynePageTitleOptions> Options { get; init; } = null!;

    /// <inheritdoc/>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenComponent<PageTitle>(0);
        builder.AddAttribute(1, nameof(PageTitle.ChildContent), (RenderFragment)BuildPageTitleChildContentRenderTree);
        builder.CloseElement();
    }

    private void BuildPageTitleChildContentRenderTree(RenderTreeBuilder builder)
    {
        var pageTitle = BuildFullPageTitle(Value, Options.Value);
        builder.AddContent(0, pageTitle);
    }

    private static string BuildFullPageTitle(string? pageTitle, TynePageTitleOptions? options)
    {
        pageTitle ??= string.Empty;

        if (options is null)
            return pageTitle;

        if (string.IsNullOrWhiteSpace(pageTitle))
            return options.Empty;

        if (string.IsNullOrWhiteSpace(options.Format))
            return pageTitle;

        return string.Format(CultureInfo.InvariantCulture, options.Format, pageTitle);
    }
}
