using ME.Web.Infrastructure.Theme;
using Microsoft.AspNetCore.Components;

namespace Tyne.Docs;

public sealed partial class App : IDisposable
{
	[Inject]
	private ThemeService ThemeService { get; init; } = default!;

	protected override async Task OnInitializedAsync()
	{
		await ThemeService.InitialiseAsync();
		ThemeService.ThemeChanged += OnThemeChangedAsync;
	}

	private async Task OnThemeChangedAsync(AppThemeType arg) =>
		await InvokeAsync(StateHasChanged);

	public void Dispose()
	{
		ThemeService.ThemeChanged -= OnThemeChangedAsync;
	}
}
