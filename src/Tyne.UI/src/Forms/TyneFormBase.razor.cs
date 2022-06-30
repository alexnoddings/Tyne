using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Tyne.Results;

namespace Tyne.UI.Forms;

/// <summary>
///		The base class for a Tyne form.
/// </summary>
/// <typeparam name="TModel">The model being edited.</typeparam>
public abstract partial class TyneFormBase<TModel> where TModel : class
{
	protected abstract ILogger LoggerInstance { get; }

	/// <summary>
	///		An optional callback that is invoked after <see cref="SaveModelAsync(TModel)"/> returns a successful <see cref="FormResult"/>.
	/// </summary>
	[Parameter]
	public EventCallback<TModel> OnSaved { get; set; }

	/// <summary>
	///		The model being edited.
	/// </summary>
	/// <remarks>
	///		This is likely to be null while the <see cref="State"/> is <see cref="FormState.Closed"/> or <see cref="FormState.Loading"/>.
	/// </remarks>
	protected TModel? ModelInstance { get; set; }

	/// <summary>
	///		The <see cref="IValidator{T}"/> for the <typeparamref name="TModel"/>.
	///		Your <typeparamref name="TModel"/> should have a <see cref="IValidator{T}"/> registered, even if it is empty.
	/// </summary>
	[Inject]
	protected virtual IValidator<TModel> Validator { get; set; } = default!;

	/// <summary>
	///		The latest <see cref="Result{Unit}"/>. This starts as <see cref="Result.Empty()"/>, and is updated once an operation completes, such as after <see cref="SaveAsync()"/>.
	/// </summary>
	protected Result<Unit> FormResult { get; set; } = Result.Empty();

	/// <summary>
	///		The state of the form. See <see cref="FormState"/> for more information.
	/// </summary>
	protected FormState State { get; set; } = FormState.Closed;

	/// <summary>
	///		The <see cref="EditForm"/> instance. This may be null while <see cref="ModelInstance"/> is being loaded, as <see cref="EditForm"/> requires a non-null object.
	/// </summary>
	protected EditForm? EditForm { get; set; }

	/// <summary>
	///		Saves changes made to the <paramref name="model"/>.
	///		Validation, logging, and callbacks are all handled by <see cref="SaveAsync()"/>.
	/// </summary>
	/// <param name="model">A reference to <see cref="ModelInstance"/>, but guaranteed to be non-null.</param>
	/// <returns>A task whose result is a <see cref="Result{Unit}"/> representing the operation.</returns>
	protected abstract Task<Result<Unit>> SaveModelAsync(TModel model);

	/// <summary>
	///		Performs the full saving process for the <see cref="ModelInstance"/>.
	///	</summary>
	///	<remarks>
	///		This:
	///		<list type="bullet">
	///			<item>Handles validation</item>
	///			<item>Calls <see cref="SaveAsync()"/></item>
	///			<item>Calls <see cref="OnSaved"/> if <see cref="SaveAsync()"/> was successful</item>
	///		</list>
	///	</remarks>
	/// <returns>
	///		<see langword="true"/> if <see cref="SaveModelAsync(TModel)"/> was called and returned a successful result, otherwise <see langword="false" /> (e.g. validation failed).
	///	</returns>
	protected virtual async Task<bool> SaveAsync()
	{
		// Can only save if the form is ready
		if (State is not FormState.Ready)
		{
			LoggerInstance.FormNotReadyToSave($"State is {State}");
			return false;
		}

		// Can't save a null model
		if (ModelInstance is null)
		{
			LoggerInstance.FormNotReadyToSave($"{nameof(ModelInstance)} is null");
			return false;
		}

		// If the EditForm or it's EditContext are null, then the form hasn't fully loaded yet
		if (EditForm?.EditContext is null)
		{
			LoggerInstance.FormNotReadyToSave($"{nameof(EditForm)}.{nameof(EditContext)} is null");
			return false;
		}

		// Ensure the EditContext is fully validated
		// The EditContext will use our Validator, so no need to call it again ourselves
		if (!EditForm.EditContext.Validate())
		{
			LoggerInstance.FormNotValid();
			return false;
		}

		// We're ready to begin saving
		State = FormState.Saving;

		LoggerInstance.FormSaving();

		// Track if saving threw an exception so we can avoid logging it again later
		var savingThrewException = false;
		try
		{
			FormResult = await SaveModelAsync(ModelInstance);
		}
		catch (Exception exception)
		{
			LoggerInstance.FormExceptionWhileSaving(exception);
			FormResult = CommonResults.UnhandledException(exception, "An error occurred while saving.");
			savingThrewException = true;
		}

		// Log failures, or invoke OnSaved if successful
		await OnSavedAsync(savingThrewException);

		// Move back to being ready
		State = FormState.Ready;

		// Return whether this was successful
		return FormResult.Success;
	}

	/// <summary>
	///		Called after <see cref="SaveAsync"/>, regardless of the result.
	///		This mainly handles calling <see cref="OnSaved"/>, and logging any results/errors.
	/// </summary>
	/// <param name="savingThrewException">Whether calling <see cref="SaveModelAsync(TModel)"/> threw an exception.</param>
	private async Task OnSavedAsync(bool savingThrewException)
	{
		if (FormResult.Success)
		{
			LoggerInstance.FormSaved();
			// This is just for the convenience of the component's parent, it doesn't need to be set
			if (OnSaved.HasDelegate)
			{
				LoggerInstance.FormInvokingSaveCallback();
				try
				{
					await OnSaved.InvokeAsync(ModelInstance);
				}
				catch (Exception exception)
				{
					LoggerInstance.FormExceptionInvokingSaveCallback(exception);
				}
			}
		}
		// Exceptions while saving are logged already, don't bother logging again
		else if (!savingThrewException)
		{
			LoggerInstance.FormFailedToSave(FormResult);
		}
	}
}
