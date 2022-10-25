using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Tyne.Results;

namespace Tyne.UI.Forms;

public abstract partial class TyneInlineForm<TModel> : TyneFormBase<TModel> where TModel : class
{
	[Inject]
	private ILogger<TyneInlineForm<TModel>> Logger { get; init; } = default!;

	protected TyneInlineForm()
	{
		State = FormState.Loading;
	}

	protected abstract Task<Result<TModel>> LoadAsync();

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender) return;

		Result<TModel> result;
		try
		{
			result = await LoadAsync();
		}
		catch (Exception exception)
		{
			Logger.FormExceptionLoadingModel(exception);
			result = CommonResults.UnhandledException<TModel>(exception, "An error occurred while loading.");
		}

		if (result.Success)
		{
			ModelInstance = result.Value;
			FormResult = Result.Successful();
			State = FormState.Ready;
		}
		else
		{
			FormResult = Result.Failure(result.Metadata);
			State = FormState.Loading;
		}

		StateHasChanged();
	}
}
