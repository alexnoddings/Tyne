using MudBlazor;

namespace Tyne.Example.Client.Infrastructure;

public class ThemeService
{
    private static readonly string[] _fontFamily = ["Atkinson Hyperlegible", "-apple-system", "BlinkMacSystemFont", "Segoe UI", "Helvetica", "Arial", "sans-serif", "Apple Color Emoji", "Segoe UI Emoji", "Segoe UI Symbol"];

    public MudTheme Theme { get; } = new()
    {
        PaletteLight = new AppPaletteLight(),
        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = ".375rem",
            AppbarHeight = "79px"
        },
        Shadows = new Shadow(),
        Typography = new Typography
        {
            Default = Normalise(new DefaultTypography
            {
                FontSize = "16px",
            }),
            H1 = NormaliseHeader(new H1Typography
            {
                FontSize = "64px",
            }),
            H2 = NormaliseHeader(new H2Typography
            {
                FontSize = "52px",
            }),
            H3 = NormaliseHeader(new H3Typography
            {
                FontSize = "38px",
            }),
            H4 = NormaliseHeader(new H4Typography
            {
                FontSize = "28px",
            }),
            H5 = Normalise(new H5Typography
            {
                FontSize = "22px",
            }),
            H6 = Normalise(new H6Typography
            {
                FontSize = "18px",
            }),
            Subtitle1 = Normalise(new Subtitle1Typography
            {
                FontSize = "14px",
            }),
            Subtitle2 = Normalise(new Subtitle2Typography
            {
                FontSize = "12px",
            }),
            Body1 = Normalise(new Body1Typography
            {
                FontSize = "16px",
            }),
            Body2 = Normalise(new Body2Typography
            {
                FontSize = "14px",
            }),
            Button = Normalise(new ButtonTypography
            {
                FontSize = "16px",
            }),
            Caption = Normalise(new CaptionTypography
            {
                FontSize = "14px",
            }),
            Overline = Normalise(new OverlineTypography
            {
                FontSize = "14px",
            })
        }
    };

    private static TTypography Normalise<TTypography>(TTypography typography) where TTypography : BaseTypography
    {
        typography.FontWeight = "400";
        typography.FontFamily = _fontFamily;
        typography.LineHeight = "1.5";
        typography.LetterSpacing = "normal";
        typography.TextTransform = string.Empty;
        return typography;
    }

    private static TTypography NormaliseHeader<TTypography>(TTypography typography) where TTypography : BaseTypography
    {
        _ = Normalise(typography);
        typography.FontWeight = "600";
        return typography;
    }
}
