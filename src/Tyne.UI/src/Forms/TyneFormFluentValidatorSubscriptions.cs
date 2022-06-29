using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using Tyne.Results;

namespace Tyne.Validation;

internal sealed class TyneFormFluentValidatorSubscriptions<TModel> : IDisposable where TModel : class
{
	private readonly EditContext _editContext;
	private readonly IValidator<TModel> _validator;
	private readonly Func<Result<Unit>?> _getCurrentResult;
	private readonly ValidationMessageStore _messages;

	public TyneFormFluentValidatorSubscriptions(EditContext editContext, IValidator<TModel> validator, Func<Result<Unit>?> getCurrentResult)
	{
		_editContext = editContext ?? throw new ArgumentNullException(nameof(editContext));
		_validator = validator ?? throw new ArgumentNullException(nameof(validator));
		_getCurrentResult = getCurrentResult ?? throw new ArgumentNullException(nameof(getCurrentResult));
		_messages = new ValidationMessageStore(editContext);

		_editContext.OnFieldChanged += OnFieldChanged;
		_editContext.OnValidationRequested += OnValidationRequested;
	}

	public void OnResultChanged()
	{
		_messages.Clear();

		var result = _getCurrentResult();
		if (result is null)
			return;

		PopulateMessages(result, null);

		_editContext.NotifyValidationStateChanged();
	}

	private void OnFieldChanged(object? sender, FieldChangedEventArgs eventArgs) =>
		ValidateField(eventArgs.FieldIdentifier);

	private void OnValidationRequested(object? sender, ValidationRequestedEventArgs e) =>
		ValidateAllFields();

	private void ValidateField(FieldIdentifier fieldIdentifier) =>
		ValidateCore(fieldIdentifier);

	private void ValidateAllFields() =>
		ValidateCore(null);

	private void ValidateCore(FieldIdentifier? fieldIdentifier)
	{
		if (_editContext.Model is not TModel model)
			return;

		var result = _getCurrentResult();
		if (result is null)
			return;

		var validationResult = _validator.Validate(model);
		var validationErrors = validationResult.Errors.AsEnumerable();
		if (fieldIdentifier is null)
		{
			_messages.Clear();
			result.Metadata.RemoveAll(metadata => metadata is IValidationErrorMetadata validationErrorMetadata);
		}
		else
		{
			_messages.Clear(fieldIdentifier.Value);
			result.Metadata.RemoveAll(metadata => metadata is IValidationErrorMetadata validationErrorMetadata && validationErrorMetadata.PropertyName == fieldIdentifier.Value.FieldName);
			validationErrors = validationErrors.Where(validationError => validationError.PropertyName == fieldIdentifier.Value.FieldName);
		}

		foreach (var fieldErrors in validationErrors.GroupBy(error => error.PropertyName))
		{
			var field = _editContext.Field(fieldErrors.Key);
			var errorMessages = fieldErrors.Select(failure => failure.ErrorMessage);
			result.Metadata.AddRange(errorMessages.Select(errorMessage => new FormValidationErrorMetadata(errorMessage, field.FieldName)));
		}

		PopulateMessages(result, fieldIdentifier?.FieldName);

		// We have to notify even if there were no messages before and are still no messages now,
		// because the "state" that changed might be the completion of some async validation task
		_editContext.NotifyValidationStateChanged();
	}

	private void PopulateMessages(Result<Unit> result, string? fieldName)
	{
        var metadata = result.Metadata.OfType<IValidationErrorMetadata>();
        if (!string.IsNullOrEmpty(fieldName))
            metadata = metadata.Where(metadata => metadata.PropertyName == fieldName);

        foreach (var fieldErrors in metadata.GroupBy(error => error.PropertyName))
		{
			var field = _editContext.Field(fieldErrors.Key);
			var errorMessages = fieldErrors.Select(failure => failure.Message);
			_messages.Add(field, errorMessages);
		}
	}

	public void Dispose()
	{
		_messages.Clear();
		_editContext.OnFieldChanged -= OnFieldChanged;
		_editContext.OnValidationRequested -= OnValidationRequested;
		_editContext.NotifyValidationStateChanged();
	}
}
