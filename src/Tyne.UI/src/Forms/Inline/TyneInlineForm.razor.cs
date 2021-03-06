namespace Tyne.UI.Forms;

public abstract partial class TyneInlineForm<TModel> : TyneFormBase<TModel> where TModel : class
{
	protected TyneInlineForm()
	{
		State = FormState.Loading;
	}

	protected abstract Task<TModel> LoadAsync();

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender) return;

		ModelInstance = await LoadAsync();
		State = FormState.Ready;
		StateHasChanged();
	}
}
