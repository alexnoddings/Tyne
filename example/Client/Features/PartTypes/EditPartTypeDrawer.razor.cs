using MediatR;
using Microsoft.AspNetCore.Components;

namespace Tyne.Aerospace.Client.Features.PartTypes;

public partial class EditPartTypeDrawer : TyneFormBase<Guid, EditPartType.Request>
{
    [Parameter]
    public ITyneTable? Table { get; set; }

    [Inject]
    private IMediator Mediator { get; init; } = null!;

    protected override async Task<Result<EditPartType.Request>> TryOpenAsync(Guid partTypeId) =>
        await Mediator.Send(new LoadEditPartType.Request { Id = partTypeId });

    protected override async Task<Result<Unit>> TrySaveAsync(EditPartType.Request request)
    {
        var result = await Mediator.Send(request);
        return result.Select(_ => Unit.Value);
    }

    protected override async Task OnSavedAsync(EditPartType.Request model)
    {
        await CloseAsync(FormCloseTrigger.FromCode);
        if (Table is not null)
            await Table.ReloadDataAsync();
    }

    private async Task DeleteAsync()
    {
        if (Model is null)
            return;

        // need to confirm this, have it check for existing parts with type, and handle the result
        await Mediator.Send(new DeletePartType.Request { Id = Model.Id });

        await CloseAsync(FormCloseTrigger.FromCode);
        if (Table is not null)
            await Table.ReloadDataAsync();
    }
}
