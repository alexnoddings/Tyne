using MudBlazor;

namespace Tyne.Aerospace.Client.Infrastructure;

public class AppPaletteLight : PaletteLight
{
    public AppPaletteLight()
    {
        const string black = "#2A1A1A";
        const string greyDark = "#554449";
        const string greyLight = "#F7F2F2";
        const string white = "#FFFFFF";

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

        Background = "#D1CDD1";
        BackgroundGray = white;
        Surface = white;
        AppbarBackground = Dark;
        AppbarText = DarkContrastText;
        DrawerBackground = white;
        DrawerText = "#464442";

        ActionDefault = "#464442";
        ActionDisabled = "#797570";
        ActionDisabledBackground = "#A5A08F";
        LinesDefault = greyDark + "33";
        LinesInputs = greyDark + "77";
        TableLines = greyDark + "77";
        TableStriped = greyLight + "AA";
        TableHover = greyDark + "1A";
        Divider = greyDark + "77";
        DividerLight = greyDark + "44";

        OverlayDark = greyDark + "22";
    }
}
