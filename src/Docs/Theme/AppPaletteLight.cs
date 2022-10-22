using MudBlazor;

namespace Tyne.Docs.Theme;

public class AppPaletteLight : Palette
{
	public AppPaletteLight()
	{
		Black = AppPalette.Black;
		White = AppPalette.White;

		Primary = AppPalette.Primary;
		PrimaryContrastText = White;
		Secondary = AppPalette.Secondary;
		SecondaryContrastText = White;
		Tertiary = AppPalette.Tertiary;
		TertiaryContrastText = White;
		Info = AppPalette.Info;
		InfoContrastText = White;
		Success = AppPalette.Success;
		SuccessContrastText = White;
		Warning = AppPalette.Warning;
		WarningContrastText = White;
		Error = AppPalette.Error;
		ErrorContrastText = White;
		Dark = "#424242";
		DarkContrastText = White;

		Background = AppPalette.GreyLight;
		BackgroundGrey = White;
		Surface = White;
		AppbarBackground = White;
		AppbarText = AppPalette.GreyDark;
		DrawerBackground = Surface;
		DrawerText = "#44403C";

		TextPrimary = "#44403C";
		TextSecondary = "#57534E";
		TextDisabled = "#78716C";
		ActionDefault = "#44403C";
		ActionDisabled = "#78716C";
		ActionDisabledBackground = "#A8A29E";

		LinesDefault = AppPalette.GreyDark + "33";
		LinesInputs = AppPalette.GreyDark + "77";
		TableLines = AppPalette.GreyDark + "77";
		TableStriped = AppPalette.GreyLight + "AA";
		TableHover = AppPalette.GreyDark + "1A";
		Divider = AppPalette.GreyDark + "77";
		DividerLight = AppPalette.GreyDark + "44";

		OverlayDark = AppPalette.GreyDark + "22";
	}
}
