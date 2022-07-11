using ME.Web.Infrastructure.Theme;
using Microsoft.AspNetCore.Components;

namespace Tyne.Docs.Shared;

public partial class AppLayout
{
	[Inject]
	private ThemeService ThemeService { get; init; } = default!;

	private bool IsNavDrawerOpen { get; set; } = true;

	private async Task ToggleThemeAsync() =>
		await ThemeService.UpdateAsync(ThemeService.ThemeType == AppThemeType.Light ? AppThemeType.Dark : AppThemeType.Light);
}
