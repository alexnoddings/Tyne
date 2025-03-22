using MudBlazor;

namespace Tyne.Example.Client.Infrastructure;

public class AppPaletteLight : PaletteLight
{
    public AppPaletteLight()
    {
        const string black = "#2A1A1A";
        const string greyDark = "#0f161c";
        const string greyLight = "#A6ACB0";
        const string white = "#F5FAFF";

        Black = black;
        White = white;

        Primary = "#207FBA";
        PrimaryContrastText = white;
        Secondary = "#B82828";
        SecondaryContrastText = white;
        Tertiary = "#CF2379";
        TertiaryContrastText = white;

        Info = "#0EB7CF";
        InfoContrastText = white;
        Success = "#60B832";
        SuccessContrastText = white;
        Warning = "#C45121";
        WarningContrastText = white;
        Error = "#B81616";
        ErrorContrastText = white;
        Dark = "#424242";
        DarkContrastText = white;

        TextPrimary = "#44404C";
        TextSecondary = "#585552";
        TextDisabled = "#777070";

        Background = white;
        BackgroundGray = greyLight;
        Surface = "#F8FBFF";
        AppbarBackground = greyDark;
        AppbarText = "#E4E9ED";
        DrawerBackground = white;
        DrawerText = "#464442";

        ActionDefault = "#464442";
        ActionDisabled = "#797570";
        ActionDisabledBackground = "#A5A08F";
        LinesDefault = greyDark + "33";
        LinesInputs = greyDark + "77";
        TableLines = greyDark + "77";
        TableStriped = greyLight + "2F";
        TableHover = greyDark + "1A";
        Divider = greyDark + "77";
        DividerLight = greyDark + "44";

        OverlayDark = greyDark + "22";
    }
}
