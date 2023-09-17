using Microsoft.AspNetCore.Components;
using Tyne.Blazor;

namespace Tyne.Aerospace.Client.Features.PartTypes;

public partial class PartTypesPage
{
    private TyneTable<SearchPartTypes.Request, SearchPartTypes.Response> Table { get; set; } = null!;
    private CreatePartTypeDrawer CreateDrawer { get; set; } = null!;
    private EditPartTypeDrawer EditDrawer { get; set; } = null!;
}
