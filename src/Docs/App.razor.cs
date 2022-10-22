using Microsoft.AspNetCore.Components;
using Tyne.Docs.Theme;

namespace Tyne.Docs;

public sealed partial class App : IDisposable
{
	[Inject]
	private ThemeService ThemeService { get; init; } = default!;

	protected override async Task OnInitializedAsync()
	{
		await ThemeService.InitialiseAsync();
		ThemeService.OnThemeChanged += OnThemeChangedAsync;
	}

	private async Task OnThemeChangedAsync(AppThemeType arg) =>
		await InvokeAsync(StateHasChanged);

	public void Dispose()
	{
		ThemeService.OnThemeChanged -= OnThemeChangedAsync;
	}
}
