using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Tyne.Aerospace.Client.Features.Systems.Filtering;

[CascadingTypeParameter(nameof(TRequest))]
public sealed partial class SimpleFilterContext<TRequest> : ComponentBase, IDisposable where TRequest : new()
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
