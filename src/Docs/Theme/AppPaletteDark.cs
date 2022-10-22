using MudBlazor;
using Tyne.Docs.Theme;

namespace Tyne.Docs.Theme;

public class AppPaletteDark : Palette
{
	public AppPaletteDark()
	{
		Black = AppPalette.Black;
		White = AppPalette.White;

		Primary = AppPalette.PrimaryAlt;
		PrimaryContrastText = White;
		Secondary = AppPalette.SecondaryAlt;
		SecondaryContrastText = White;
		Tertiary = AppPalette.TertiaryAlt;
		TertiaryContrastText = White;
		Info = AppPalette.Info;
		InfoContrastText = White;
		Success = AppPalette.Success;
		SuccessContrastText = White;
		Warning = AppPalette.Warning;
		WarningContrastText = White;
		Error = AppPalette.Error;
		ErrorContrastText = White;
		Dark = "#808080";
		DarkContrastText = White;

		Background = Black;
		BackgroundGrey = AppPalette.GreyDark;
		Surface = AppPalette.GreyDark;
		AppbarBackground = AppPalette.GreyDark;
		AppbarText = AppPalette.GreyLight;
		DrawerBackground = Surface;
		DrawerText = White;

		TextPrimary = White;
		TextSecondary = "#D2D2D2";
		TextDisabled = "#929292";
		ActionDefault = White;
		ActionDisabled = "#929292";
		ActionDisabledBackground = "#666666";

		LinesDefault = AppPalette.GreyLight + "33";
		LinesInputs = AppPalette.GreyLight + "77";
		TableLines = AppPalette.GreyLight + "77";
		TableStriped = AppPalette.GreyLight + "10";
		TableHover = AppPalette.Black + "CC";
		Divider = AppPalette.GreyLight + "77";
		DividerLight = AppPalette.GreyLight + "44";

		OverlayDark = AppPalette.GreyDark + "88";
	}
}
