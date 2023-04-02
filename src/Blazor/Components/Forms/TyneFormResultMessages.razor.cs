using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor;

public partial class TyneFormResultMessages
{
    [Parameter, EditorRequired]
    public IResult? Result { get; set; }

    private bool _shouldRender;
    private bool _hasSuccessMessages;
    private bool _hasErrorMessages;
    private bool _hasWarningMessages;
    private bool _hasInfoMessages;

    protected override void OnParametersSet()
    {
        if (Result is null)
        {
            _hasSuccessMessages = _hasErrorMessages = _hasWarningMessages = _hasInfoMessages = _shouldRender = false;
            return;
        }

        _hasSuccessMessages = Result.Messages.Any(message => message.Type is ResultMessageType.Success);
        _hasErrorMessages = Result.Messages.Any(message => message.Type is ResultMessageType.Error);
        _hasWarningMessages = Result.Messages.Any(message => message.Type is ResultMessageType.Warning);
        _hasInfoMessages = Result.Messages.Any(message => message.Type is ResultMessageType.Info);
        _shouldRender = _hasSuccessMessages || _hasErrorMessages || _hasInfoMessages;
    }

    private IEnumerable<string> OfType(ResultMessageType type) =>
        Result is null
        ? Enumerable.Empty<string>()
        : Result.Messages
            .Where(message => message.Type == type)
            .Select(message => message.Message);
}
