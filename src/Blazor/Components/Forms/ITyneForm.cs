using MediatR;
using Microsoft.AspNetCore.Components.Forms;

namespace Tyne.Blazor;

public interface ITyneForm
{
    public FormState State { get; }
    /// <summary>
	///		The <see cref="Microsoft.AspNetCore.Components.Forms.EditForm"/> instance. This may be null while <see cref="Model"/> is being loaded, as it requires a non-null object.
	/// </summary>
	public EditForm? EditForm { get; set; }

    public Result<Unit>? InitialiseResult { get; }
    public Result<Unit>? SaveResult { get; }

    public IDisposable Attach(FormUpdatedCallback formUpdatedCallback);
    public Task SaveAsync();
    public Task CloseAsync(FormCloseTrigger closeTrigger);
}
