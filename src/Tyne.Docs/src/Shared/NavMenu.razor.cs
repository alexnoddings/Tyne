using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Tyne.Docs.Services.Pages;

namespace Tyne.Docs.Shared;

public sealed partial class NavMenu : IDisposable
{
	[Inject]
	private NavigationManager NavigationManager { get; init; } = default!;

	private string UriStart { get; set; } = string.Empty;

	private static List<PageInfo> CorePages { get; } = PageService.GetPages(PageCategory.Core).ToList();
	private static List<PageInfo> EfPages { get; } = PageService.GetPages(PageCategory.EF).ToList();
	private static List<PageInfo> UiPages { get; } = PageService.GetPages(PageCategory.UI).ToList();

	protected override void OnInitialized()
	{
		UpdateUriStart();
		NavigationManager.LocationChanged += OnNavigationManagerLocationChanged;
	}

	private void OnNavigationManagerLocationChanged(object? sender, LocationChangedEventArgs e)
	{
		UpdateUriStart();
		InvokeAsync(StateHasChanged);
	}

	private void UpdateUriStart()
	{
		var path = new Uri(NavigationManager.Uri).AbsolutePath;
		UriStart = path
			.Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
			.FirstOrDefault() ?? string.Empty;
	}

	public void Dispose()
	{
		NavigationManager.LocationChanged -= OnNavigationManagerLocationChanged;
	}
}
