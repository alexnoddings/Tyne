using System.Diagnostics.CodeAnalysis;
using System.Web;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Tyne.Aerospace.Client.Features.Theme;

public partial class ThemePage
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private Typo _selectedTypo = Typo.body1;
    private Typo SelectedTypo
    {
        get => _selectedTypo;
        set
        {
            if (_selectedTypo == value)
                return;
            _selectedTypo = value;
            var newUri = NavigationManager.GetUriWithQueryParameter("typo", value.ToString());
            NavigationManager.NavigateTo(newUri);
        }
    }

    private string FontSizeText { get; set; } = "The quick brown fox jumps over the lazy dog";

    protected override void OnInitialized()
    {
        var query = new Uri(NavigationManager.Uri).Query;
        var typoString = HttpUtility.ParseQueryString(query).GetValues("typo")?.FirstOrDefault();
        if (Enum.TryParse(typoString, out Typo typo))
            _selectedTypo = typo;
    }

    [SuppressMessage("Style", "IDE0072: Add missing cases.", Justification = "Other cases are handled by the default branch.")]
    private int Md => SelectedTypo switch
    {
        Typo.inherit => 12,
        Typo.h1 => 12,
        Typo.h2 => 6,
        Typo.h3 => 6,
        Typo.h4 => 4,
        _ => 3,
    };

    private static IEnumerable<Color> GetColours()
    {
        foreach (var colour1 in GetColours1())
            yield return colour1;
        foreach (var colour2 in GetColours2())
            yield return colour2;
    }

    private static IEnumerable<Color> GetColours1()
    {
        yield return Color.Primary;
        yield return Color.Secondary;
        yield return Color.Tertiary;
        yield return Color.Dark;
    }

    private static IEnumerable<Color> GetColours2()
    {
        yield return Color.Info;
        yield return Color.Success;
        yield return Color.Warning;
        yield return Color.Error;
    }

    // Used to return colours twice to show each colour against alternating stripes in the table
    private static IEnumerable<T> GetTwice<T>(Func<IEnumerable<T>> enumerableFunc)
    {
        foreach (var value in enumerableFunc())
        {
            yield return value;
            yield return value;
        }
    }

    private static IEnumerable<string> GetTextTypes()
    {
        yield return "Primary";
        yield return "Secondary";
        yield return "Disabled";
    }
}
