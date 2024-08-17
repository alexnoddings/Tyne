using MudBlazor;

namespace Tyne.Aerospace.Client.Infrastructure;

public class ThemeService
{
    private static readonly string[] _fontFamily = ["Atkinson Hyperlegible", "-apple-system", "BlinkMacSystemFont", "Segoe UI", "Helvetica", "Arial", "sans-serif", "Apple Color Emoji", "Segoe UI Emoji", "Segoe UI Symbol"];

    public MudTheme Theme { get; } = new()
    {
        PaletteLight = new AppPaletteLight(),
        LayoutProperties = new LayoutProperties()
        {
            DefaultBorderRadius = "8px",
            AppbarHeight = "74px"
        },
        Shadows = new Shadow(),
        Typography = new Typography
        {
            Default = Normalise(new Default
            {
                FontSize = "16px",
            }),
            H1 = NormaliseHeader(new H1
            {
                FontSize = "64px",
            }),
            H2 = NormaliseHeader(new H2
            {
                FontSize = "52px",
            }),
            H3 = NormaliseHeader(new H3
            {
                FontSize = "38px",
            }),
            H4 = NormaliseHeader(new H4
            {
                FontSize = "28px",
            }),
            H5 = Normalise(new H5
            {
                FontSize = "22px",
            }),
            H6 = Normalise(new H6
            {
                FontSize = "18px",
            }),
            Subtitle1 = Normalise(new Subtitle1
            {
                FontSize = "14px",
            }),
            Subtitle2 = Normalise(new Subtitle2
            {
                FontSize = "12px",
            }),
            Body1 = Normalise(new Body1
            {
                FontSize = "16px",
            }),
            Body2 = Normalise(new Body2
            {
                FontSize = "14px",
            }),
            Button = Normalise(new Button
            {
                FontSize = "16px",
            }),
            Caption = Normalise(new Caption
            {
                FontSize = "14px",
            }),
            Overline = Normalise(new Overline
            {
                FontSize = "14px",
            })
        }
    };

    private static TTypography Normalise<TTypography>(TTypography typography) where TTypography : BaseTypography
    {
        typography.FontWeight = 400;
        typography.FontFamily = _fontFamily;
        typography.LineHeight = 1.5;
        typography.LetterSpacing = "normal";
        typography.TextTransform = string.Empty;
        return typography;
    }

    private static TTypography NormaliseHeader<TTypography>(TTypography typography) where TTypography : BaseTypography
    {
        _ = Normalise(typography);
        typography.FontWeight = 600;
        return typography;
    }
}
