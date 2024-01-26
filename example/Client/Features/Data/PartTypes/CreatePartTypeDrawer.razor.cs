using MediatR;
using Microsoft.AspNetCore.Components;

namespace Tyne.Aerospace.Client.Features.Data.PartTypes;

public partial class CreatePartTypeDrawer : TyneFormBase<Unit, CreatePartType.Request>
{
    [Parameter]
    public ITyneTable? Table { get; set; }

    [Inject]
    private IMediator Mediator { get; init; } = null!;

    protected override Task<Result<CreatePartType.Request>> TryOpenAsync(Unit _) =>
        Ok(new CreatePartType.Request()).ToTask();

    protected override async Task<Result<Unit>> TrySaveAsync(CreatePartType.Request request)
    {
        return await Mediator.Send(request);
    }

    protected override async Task OnSavedAsync(CreatePartType.Request model)
    {
        await CloseAsync(FormCloseTrigger.FromCode);

        if (Table is not null)
            await Table.ReloadDataAsync();
    }
}