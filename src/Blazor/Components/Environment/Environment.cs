using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Tyne.Blazor;

/// <summary>
///     Conditionally renders <see cref="ChildContent"/> based on the current <see cref="IEnvironment.EnvironmentName"/>.
/// </summary>
public partial class Environment : ComponentBase
{
    [Inject]
    private IEnvironment AppEnvironment { get; set; } = default!;

    /// <summary>
    ///     Content to render when the environment names matches <see cref="Filter"/>.
    ///     This takes the current environment name as a context.
    /// </summary>
    [Parameter]
    public RenderFragment<string> ChildContent { get; set; } = default!;

    /// <summary>
    ///     The <see cref="EnvironmentMode"/> which should be used when checking <see cref="IEnvironment.EnvironmentName"/>.
    /// </summary>
    /// <remarks>
    ///     See <see cref="EnvironmentMode"/> for how the mode works. Defaults to <see cref="EnvironmentMode.Include"/>.
    /// </remarks>
    [Parameter, EditorRequired]
    public EnvironmentMode Mode { get; set; } = EnvironmentMode.Include;

    /// <summary>
    ///     The <see cref="EnvironmentMatching"/> which should be used when matching <see cref="IEnvironment.EnvironmentName"/>.
    /// </summary>
    /// <remarks>
    ///     See <see cref="EnvironmentMatching"/> for how the matching works. Defaults to <see cref="EnvironmentMatching.Exact"/>.
    /// </remarks>
    [Parameter]
    public EnvironmentMatching Matching { get; set; } = EnvironmentMatching.Exact;

    /// <summary>
    ///     A comma-separated list of environment names to match.
    /// </summary>
    /// <remarks>
    ///     See <see cref="Mode"/> and <see cref="Matching"/>.
    /// </remarks>
    [Parameter, EditorRequired]
    public string Filter { get; set; } = string.Empty;

    private string[] _environmentNormalisedNameFilters = Array.Empty<string>();

    private string _environmentName = string.Empty;
    private string _environmentNormalisedName = string.Empty;

    protected override void OnInitialized()
    {
        // Cache environment name as it won't change for the lifetime of the component
        _environmentName = AppEnvironment.EnvironmentName;
        // Normalise the environment name by trimming whitespace and converting to uppercase (the same as how the filter is processed)
        _environmentNormalisedName = _environmentName.Trim().ToUpperInvariant();
    }

    protected override void OnParametersSet()
    {
        if (!Enum.IsDefined(Mode))
            throw new InvalidOperationException($"Undefined value for {nameof(Mode)}: {Mode}.");

        if (!Enum.IsDefined(Matching))
            throw new InvalidOperationException($"Undefined value for {nameof(Matching)}: {Matching}.");

        if (Filter is null)
            throw new InvalidOperationException($"Cannot provide a null value for {nameof(Filter)}.");

        // Cache environment names to avoid the overhead of processing Filter every render
        _environmentNormalisedNameFilters = Filter
            .ToUpperInvariant()
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var envName = _environmentNormalisedName;
        var filters = _environmentNormalisedNameFilters;
        var matches = Matching switch
        {
            EnvironmentMatching.Exact => filters.Any(nameFilter => envName.Equals(nameFilter, StringComparison.Ordinal)),
            EnvironmentMatching.Contains => filters.Any(nameFilter => envName.Contains(nameFilter, StringComparison.Ordinal)),
            _ => throw new InvalidOperationException($"Undefined value for {nameof(Matching)}: {Matching}.")
        };

        var shouldRender = (matches && Mode is EnvironmentMode.Include) || (!matches && Mode is EnvironmentMode.Exclude);
        if (shouldRender)
            builder.AddContent(0, ChildContent, _environmentName);
    }
}
