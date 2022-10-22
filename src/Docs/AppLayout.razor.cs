using Microsoft.AspNetCore.Components;
using Tyne.Docs.Theme;

namespace Tyne.Docs;

public sealed partial class AppLayout : IDisposable
{
	[Inject]
	private ThemeService ThemeService { get; init; } = default!;

	private bool IsNavDrawerOpen { get; set; } = true;

	protected override void OnInitialized()
	{
		ThemeService.OnThemeChanged += OnThemeChangedAsync;
	}

	private async Task ToggleThemeAsync() =>
		await ThemeService.UpdateAsync(ThemeService.ThemeType == AppThemeType.Light ? AppThemeType.Dark : AppThemeType.Light);

	private async Task OnThemeChangedAsync(AppThemeType arg) =>
		await InvokeAsync(StateHasChanged);

	public void Dispose()
	{
		ThemeService.OnThemeChanged -= OnThemeChangedAsync;
	}
}
