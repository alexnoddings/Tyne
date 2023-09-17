using MudBlazor;

namespace Tyne.Aerospace.Client.Infrastructure;

public class ThemeService
{
	public MudTheme Theme { get; }

	private static readonly string[] FontFamily = ["Atkinson Hyperlegible", "-apple-system", "BlinkMacSystemFont", "Segoe UI", "Helvetica", "Arial", "sans-serif", "Apple Color Emoji", "Segoe UI Emoji", "Segoe UI Symbol"];

	public ThemeService()
	{
		Theme = new MudTheme
		{
			Palette = new AppPaletteLight(),
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
					FontWeight = 400,
					FontSize = "14px",
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
					FontSize = "14px",
				}),
				Body2 = Normalise(new Body2
				{
					FontSize = "12px",
				}),
				Button = Normalise(new Button
				{
					FontSize = "14px",
				}),
				Caption = Normalise(new Caption
				{
				}),
				Overline = Normalise(new Overline
				{
				})
			}
		};
	}

	private static TTypography Normalise<TTypography>(TTypography typography) where TTypography : BaseTypography
	{
		typography.FontFamily = FontFamily;
		typography.LineHeight = 1.5;
		typography.LetterSpacing = "normal";
		typography.TextTransform = string.Empty;
		return typography;
	}

	private static TTypography NormaliseHeader<TTypography>(TTypography typography) where TTypography : BaseTypography
	{
		typography.FontWeight = 600;
		return Normalise(typography);
	}
}
