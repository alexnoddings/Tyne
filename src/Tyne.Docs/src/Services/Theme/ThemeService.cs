using Blazored.LocalStorage;
using MudBlazor;

namespace ME.Web.Infrastructure.Theme;

public class ThemeService
{
	public const string AppThemeKey = "tyne.docs.theme";

	public MudTheme Theme { get; }
	public AppThemeType ThemeType { get; private set; }

	public event Func<AppThemeType, Task>? ThemeChanged;

	private ILocalStorageService LocalStorageService { get; }

	public ThemeService(ILocalStorageService localStorageService)
	{
		LocalStorageService = localStorageService ?? throw new ArgumentNullException(nameof(localStorageService));

		var fontFamily = new string[] { "-apple-system", "BlinkMacSystemFont", "Lato", "Segoe UI", "Helvetica", "Arial", "sans-serif" };
		var fontWeightHeader = 400;
		var fontWeightBody = 300;

		Theme = new MudTheme
		{
			Palette = new AppPaletteLight(),
			PaletteDark = new AppPaletteDark(),

			Shadows = new Shadow(),
			Typography = new Typography
			{
				Default = new Default
				{
					FontFamily = fontFamily,
					FontWeight = fontWeightBody,
				},
				H1 = new H1
				{
					FontWeight = fontWeightHeader,
				},
				H2 = new H2
				{
					FontWeight = fontWeightHeader,
				},
				H3 = new H3
				{
					FontWeight = fontWeightHeader,
				},
				H4 = new H4
				{
					FontWeight = fontWeightHeader,
				},
				H5 = new H5
				{
					FontWeight = fontWeightHeader,
				},
				H6 = new H6
				{
					FontWeight = fontWeightHeader,
				},
			},
			LayoutProperties = new LayoutProperties() { DefaultBorderRadius = "0px" },
			ZIndex = new ZIndex(),
		};
	}

	public async Task InitialiseAsync()
	{
		var themeType = await LocalStorageService.GetItemAsync<AppThemeType?>(AppThemeKey);
		if (themeType is null)
		{
			themeType = AppThemeType.Light;
			await LocalStorageService.SetItemAsync(AppThemeKey, themeType);
		}

		ThemeType = themeType.Value;
	}

	public Task UpdateAsync(AppThemeType themeType)
	{
		if (!Enum.IsDefined(themeType))
			throw new ArgumentOutOfRangeException(nameof(themeType));
		return UpdateAsyncCore(themeType);
	}

	private async Task UpdateAsyncCore(AppThemeType themeType)
	{
		await LocalStorageService.SetItemAsync(AppThemeKey, themeType);

		ThemeType = themeType;

		if (ThemeChanged is not null)
			await ThemeChanged.Invoke(themeType);
	}
}
