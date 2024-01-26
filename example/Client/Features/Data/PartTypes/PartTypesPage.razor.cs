namespace Tyne.Aerospace.Client.Features.Data.PartTypes;

public partial class PartTypesPage
{
    private ITyneTable Table { get; set; } = null!;
    private CreatePartTypeDrawer CreateDrawer { get; set; } = null!;
    private EditPartTypeDrawer EditDrawer { get; set; } = null!;
}
