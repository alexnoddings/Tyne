using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Logging;

namespace Tyne.Aerospace.Client.Features.Systems.Filtering;

[CascadingTypeParameter(nameof(TRequest))]
public sealed class SimpleFilterContext<TRequest> : ComponentBase, IDisposable where TRequest : new()
{
    [Inject]
    private ILoggerFactory LoggerFactory { get; init; } = null!;

    [Inject]
    private IUrlPersistenceService PersistenceService { get; init; } = null!;

    [Parameter]
    public RenderFragment FilterValues { get; set; } = null!;

    [Parameter]
    public RenderFragment<TRequest> ChildContent { get; set; } = null!;

    private TRequest? _filterContextConfiguredRequest;
    private TyneFilterContext<TRequest> _filterContext = null!;

    protected override void OnInitialized()
    {
        var contextLogger = LoggerFactory.CreateLogger<TyneFilterContext<TRequest>>();
        _filterContext = new(contextLogger, PersistenceService, OnFilterContextReloadedAsync);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;

        await _filterContext.InitialiseAsync();
        await ReloadConfiguredRequestAsync();
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenComponent<CascadingValue<IFilterContext<TRequest>>>(0);
        builder.AddAttribute(1, nameof(CascadingValue<object>.Value), _filterContext);
        builder.AddAttribute(2, nameof(CascadingValue<object>.IsFixed), true);
        builder.AddAttribute(3, nameof(CascadingValue<object>.ChildContent), (RenderFragment)RenderContent);
        builder.CloseComponent();

        void RenderContent(RenderTreeBuilder innerBuilder)
        {
            innerBuilder.AddContent(0, FilterValues);
            if (_filterContextConfiguredRequest is not null)
                innerBuilder.AddContent(1, ChildContent(_filterContextConfiguredRequest));
        }
    }

    private async Task ReloadConfiguredRequestAsync()
    {
        _filterContextConfiguredRequest = new();
        await _filterContext.ConfigureRequestAsync(_filterContextConfiguredRequest);
        await InvokeAsync(StateHasChanged);
    }

    private Task OnFilterContextReloadedAsync() =>
        ReloadConfiguredRequestAsync();

    public void Dispose() =>
        _filterContext?.Dispose();
}
