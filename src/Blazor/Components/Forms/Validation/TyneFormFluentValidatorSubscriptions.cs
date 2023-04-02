using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;

namespace Tyne.Blazor;

internal sealed class TyneFormFluentValidatorSubscriptions<TModel> : IDisposable where TModel : class
{
    private readonly EditContext _editContext;
    private readonly IEnumerable<IValidator<TModel>> _validators;
    private readonly ValidationMessageStore _messages;

    public FormValidationEvents ValidationEvents { get; set; }

    public TyneFormFluentValidatorSubscriptions(EditContext editContext, IEnumerable<IValidator<TModel>> validators)
    {
        _editContext = editContext ?? throw new ArgumentNullException(nameof(editContext));
        _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        _messages = new ValidationMessageStore(editContext);

        _editContext.OnFieldChanged += OnFieldChanged;
        _editContext.OnValidationRequested += OnValidationRequested;
    }

    private void OnFieldChanged(object? sender, FieldChangedEventArgs eventArgs)
    {
        if (ValidationEvents.HasFlag(FormValidationEvents.OnFieldChanged))
            ValidateCore(eventArgs.FieldIdentifier);
    }

    private void OnValidationRequested(object? sender, ValidationRequestedEventArgs eventArgs)
    {
        if (ValidationEvents.HasFlag(FormValidationEvents.OnSaveRequested))
            ValidateCore(null);
    }

    private void ValidateCore(FieldIdentifier? fieldIdentifier)
    {
        if (_editContext.Model is not TModel model)
            return;

        var validationContext =
            fieldIdentifier is not null
            // If a fieldIdentifier is passed, only validate the property it identifies
            ? ValidationContext<TModel>.CreateWithOptions(model, options => options.IncludeProperties(fieldIdentifier.Value.FieldName))
            // Otherwise, validate all properties
            : new ValidationContext<TModel>(model);

        var propertyValidationErrors = _validators
            .Select(validator => validator.Validate(validationContext))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(error => error.Severity is Severity.Error)
            .GroupBy(error => error.PropertyName);

        if (fieldIdentifier is not null)
            _messages.Clear(fieldIdentifier.Value);
        else
            _messages.Clear();

        foreach (var fieldErrors in propertyValidationErrors)
        {
            var field = _editContext.Field(fieldErrors.Key);
            var errorMessages = fieldErrors.Select(failure => failure.ErrorMessage);
            _messages.Add(field, errorMessages);
        }

        // We have to notify even if there were no messages before and are still no messages now,
        // because the "state" that changed might be the completion of some async validation task
        _editContext.NotifyValidationStateChanged();
    }

    public void Dispose()
    {
        _messages.Clear();
        _editContext.OnFieldChanged -= OnFieldChanged;
        _editContext.OnValidationRequested -= OnValidationRequested;
        _editContext.NotifyValidationStateChanged();
    }
}