using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MudBlazor.Utilities;
using Tyne.Results;

namespace Tyne.UI.Forms;

public abstract partial class TyneModalForm<TOpen, TModel> : TyneFormBase<TModel> where TModel : class
{
	[Inject]
	private ILogger<TyneModalForm<TOpen, TModel>> Logger { get; init; } = default!;

	protected virtual bool ShouldCloseOnClickBehind => false;

	protected virtual bool ShouldCloseOnSave => false;
	
	protected string Classname =>
		new CssBuilder()
		.AddClass(OpenSpeedClass)
		.AddClass(CloseSpeedClass)
		.Build();

	protected string OpenSpeedClass =>
		OpenSpeed switch
		{
			ModalAnimationSpeed.Skipped => "tyne-drawer-open-skip",
			ModalAnimationSpeed.Fast => "tyne-drawer-open-fast",
			ModalAnimationSpeed.Normal => "tyne-drawer-open-normal",
			ModalAnimationSpeed.Slow => "tyne-drawer-open-slow",
			_ => "tyne-drawer-open-normal"
		};

	protected virtual ModalAnimationSpeed OpenSpeed => ModalAnimationSpeed.Normal;

	protected string CloseSpeedClass =>
		CloseSpeed switch
		{
			ModalAnimationSpeed.Skipped => "tyne-drawer-close-skip",
			ModalAnimationSpeed.Fast => "tyne-drawer-close-fast",
			ModalAnimationSpeed.Normal => "tyne-drawer-close-normal",
			ModalAnimationSpeed.Slow => "tyne-drawer-close-slow",
			_ => "tyne-drawer-close-normal"
		};

	protected virtual ModalAnimationSpeed CloseSpeed => ModalAnimationSpeed.Fast;

    private Task? ModalInitialisingTask { get; set; }
    private Result<Unit>? ModalInitialisationResult { get; set; }

    /// <summary>
    ///     Called when the modal is initialising.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This will be executed during <see cref="OnAfterRenderAsync(bool)"/> during the first render.
    ///         This is to prevent it from running twice, as would be the case with <see cref="ComponentBase.OnInitializedAsync"/> if using pre-rendering.
    ///     </para>
    ///     <para>
    ///         Unlike <see cref="ComponentBase.OnInitializedAsync"/> or <see cref="OnAfterRenderAsync(bool)"/>, this is guaranteed to be <see langword="await"/>ed before <see cref="LoadModelAsync(TOpen)"/> is executed.
    ///     </para>
    /// </remarks>
    protected virtual Task OnModalFirstLoadAsync() => Task.CompletedTask;
    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return Task.CompletedTask;

        // Kick off the modal initialising task
        ModalInitialisingTask = OnModalFirstLoadAsync();
        return Task.CompletedTask;
    }

    // Tracks an in-progress opening operation. Used to cancel opening
    // to prevent a modal from re-opening once data is loaded.
    protected CancellationTokenSource? OpeningTokenSource { get; set; } = new();

    protected abstract Task<Result<TModel>> LoadModelAsync(TOpen openInput);

	public async Task OpenAsync(TOpen openInput)
	{
        // Reset the previous form result
        FormResult = Result.Empty();
        // Move to the loading state
		State = FormState.Loading;
        // Cancel any previous opening operations
        OpeningTokenSource?.Cancel();
        // Create a new opening operation token source
        var thisOpeningTokenSource = new CancellationTokenSource();
        OpeningTokenSource = thisOpeningTokenSource;

        // If modal initialisation has previously failed, then re-display that error.
        // We don't want to try and recover from the failure, as it showing the form anyway could corrupt data,
        // and trying to re-execute the loading could create an undefined state.
        if (ModalInitialisationResult?.Success == false)
        {
            FormResult = Result.Failure(ModalInitialisationResult.Metadata);
            State = FormState.Ready;
            return;
        }

        // Update the UI to start opening the panel
        StateHasChanged();

        // Make sure the first load task has finished before trying to load the model
        if (ModalInitialisingTask is not null)
        {
            try
            {
                await ModalInitialisingTask;
                ModalInitialisationResult = Result.Successful();
            }
            catch (Exception exception)
            {
                // Discard the task
                ModalInitialisingTask = null;
                // Set the initialisation result as a failure, which will prevent the form from opening
                ModalInitialisationResult = CommonResults.UnhandledException(exception, "An error occurred while loading");
				Logger.FormInitialisingException(exception);

                // If this opening has been cancelled elsewhere, don't bother updating anything
                if (thisOpeningTokenSource.IsCancellationRequested)
                    return;

                FormResult = Result.Failure(ModalInitialisationResult.Metadata);
                State = FormState.Ready;
                return;
            }
        }

        Result<TModel> loadModelResult;
        try
		{
            loadModelResult = await LoadModelAsync(openInput);
		}
		catch (Exception exception)
		{
			Logger.FormExceptionLoadingModel(exception);

            // If this opening has been cancelled elsewhere, don't bother updating anything
            if (thisOpeningTokenSource.IsCancellationRequested)
                return;

			FormResult = CommonResults.UnhandledException(exception, "An error occurred while loading.");
            State = FormState.Ready;
            StateHasChanged();
            return;
        }

        // If this opening has been cancelled elsewhere, don't bother updating anything
        if (thisOpeningTokenSource.IsCancellationRequested)
            return;

        if (loadModelResult.Success)
        {
            // Update the model instance with the result
            ModelInstance = loadModelResult.Value;
            // Update the result as a success, copying the load result's metadata
            FormResult = Result.Successful(loadModelResult.Metadata);
        }
        else
        {
			Logger.FormFailedToLoad(loadModelResult);
            // Update the result as a failure, copying the load result's metadata
            FormResult = Result.Failure(loadModelResult.Metadata);
        }

        // Set the form as ready
        State = FormState.Ready;
        // Update the UI
		StateHasChanged();

		await OnOpenedAsync(openInput, loadModelResult.Success);
	}

	protected virtual Task OnOpenedAsync(TOpen openInput, bool openedSuccessfully) => Task.CompletedTask;

	protected virtual async Task OnOpenChanged(bool shouldBeOpen)
	{
        // If State is Closed, we have already closed the form
        // If State is not Closed, and shouldBeOpen is false, the user is trying to dismiss the panel by clicking behind it
        // If ShouldCloseOnClickBehind is true, then allow the modal to be dismissed, otherwise ignore the event
        if (State is not FormState.Closed && !shouldBeOpen && ShouldCloseOnClickBehind)
			await CloseAsync(ModalCloseReason.Dismissed);
	}

    protected virtual Task OnClosedAsync(ModalCloseReason reason) => Task.CompletedTask;

	public async Task CloseAsync(ModalCloseReason reason)
	{
        // Update the form state to closed
		State = FormState.Closed;
        // Null the model instance
		ModelInstance = null;
        // Cancel and null any opening operation
        OpeningTokenSource?.Cancel();
        OpeningTokenSource = null;

        await OnClosedAsync(reason);
	}

	protected override async Task<bool> SaveAsync()
	{
        // Don't bother trying to save if the modal wasn't initialised successfully
        if (ModalInitialisationResult?.Success == false)
            return false;

		var saveSucceeded = await base.SaveAsync();
		if (saveSucceeded && ShouldCloseOnSave)
			await CloseAsync(ModalCloseReason.Saved);
		return saveSucceeded;
	}
}
